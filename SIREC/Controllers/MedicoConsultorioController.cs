using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SIREC.Data;
using SIREC.Models;

namespace SIREC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoConsultorioController : ControllerBase
    {
        private readonly SirecDbContext _context;

        public MedicoConsultorioController(SirecDbContext context)
        {
            _context = context;
        }

        // POST: api/MedicoConsultorio
        [HttpPost]
        public async Task<IActionResult> AsignarMedico([FromBody] MedicoConsultorio asignacion)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.MedicoConsultorios.Add(asignacion);
            await _context.SaveChangesAsync();
            return Ok(asignacion);
        }

        // GET: api/MedicoConsultorio
        [HttpGet]
        public async Task<IActionResult> ObtenerAsignaciones()
        {
            var asignaciones = await _context.MedicoConsultorios
                .Include(mc => mc.Usuario)
                .Include(mc => mc.Consultorio)
                .ToListAsync();

            return Ok(asignaciones);
        }

        // GET: api/MedicoConsultorio/usuario/13
        [HttpGet("usuario/{idUsuario}")]
        public async Task<IActionResult> ObtenerConsultoriosPorMedico(int idUsuario)
        {
            var consultorios = await _context.MedicoConsultorios
                .Where(mc => mc.IDUsuario == idUsuario)
                .Include(mc => mc.Consultorio)
                .Select(mc => mc.Consultorio)
                .ToListAsync();

            return Ok(consultorios);
        }

        // DELETE: api/MedicoConsultorio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAsignacion(int id)
        {
            var asignacion = await _context.MedicoConsultorios.FindAsync(id);
            if (asignacion == null)
                return NotFound();

            _context.MedicoConsultorios.Remove(asignacion);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}