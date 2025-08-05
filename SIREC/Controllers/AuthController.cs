using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIREC.Data;
using SIREC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIREC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SirecDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(SirecDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario, [FromQuery] int idRol = 2)
        {
            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var usuarioRol = new UsuarioRol
                {
                    IDUsuario = usuario.IDUsuario,
                    IDRol = idRol
                };

                _context.UsuarioRoles.Add(usuarioRol);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Usuario registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al registrar", detalle = ex.Message });
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.Correo == request.Correo && u.Contraseña == request.Contraseña);

            if (usuario == null)
                return Unauthorized();

            var rol = usuario.UsuarioRoles.FirstOrDefault()?.Rol?.Nombre ?? "Paciente";

            var claims = new[]
            {
                new Claim("id", usuario.IDUsuario.ToString()),
                new Claim("role", rol),
                new Claim(ClaimTypes.NameIdentifier, usuario.IDUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

    }
}