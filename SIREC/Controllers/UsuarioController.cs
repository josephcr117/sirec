using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIREC.Data;
using SIREC.Models;

namespace SIREC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly SirecDbContext _context;

        public UsuarioController(SirecDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .ToListAsync();

            var resultado = usuarios.Select(u => new {
                id = u.IDUsuario,
                nombre = $"{u.Nombre} {u.Apellido}",
                correo = u.Correo,
                roles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList()
            });

            return Ok(resultado);
        }

        // GET: api/usuario/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .FirstOrDefaultAsync(u => u.IDUsuario == id);

            if (usuario == null)
                return NotFound();

            var resultado = new
            {
                id = usuario.IDUsuario,
                nombre = $"{usuario.Nombre} {usuario.Apellido}",
                correo = usuario.Correo,
                roles = usuario.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList()
            };

            return Ok(resultado);
        }
        
        // GET: api/usuario/medicos
        [HttpGet("medicos")]
        public async Task<IActionResult> GetMedicos()
        {
            var medicos = await _context.Usuarios
                .Include(u => u.UsuarioRoles)
                    .ThenInclude(ur => ur.Rol)
                .Where(u => u.UsuarioRoles.Any(ur => ur.IDRol == 3))
                .ToListAsync();

            var resultado = medicos.Select(u => new
            {
                id = u.IDUsuario,
                nombre = $"{u.Nombre} {u.Apellido}",
                correo = u.Correo,
                roles = u.UsuarioRoles.Select(ur => ur.Rol.Nombre).ToList()
            });

            return Ok(resultado);
        }

        // POST: api/usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IDUsuario }, usuario);
        }

        // PUT: api/usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IDUsuario)
                return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.IDUsuario == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}