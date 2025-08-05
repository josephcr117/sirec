using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIREC.Data;
using SIREC.Models;
using System.Security.Claims;
using System.Text.Json;

namespace SIREC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly SirecDbContext _context;

        public CitasController(SirecDbContext context)
        {
            _context = context;
        }

        // GET: api/Citas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitas()
        {
            return await _context.Citas
                .Include(c => c.Consultorio)
                .Include(c => c.Usuario)
                .ToListAsync();
        }

        // GET: api/Citas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            var cita = await _context.Citas
                .Include(c => c.Consultorio)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.IDCita == id);

            if (cita == null)
                return NotFound();

            return cita;
        }

        // GET: api/Citas/usuario/3
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<IEnumerable<Cita>>> GetCitasPorUsuario(int idUsuario)
        {
            return await _context.Citas
                .Where(c => c.IDUsuario == idUsuario)
                .Include(c => c.Consultorio)
                .ToListAsync();
        }

        // GET: api/Citas/medico/5
        [HttpGet("medico/{idUsuario}")]
        public async Task<IActionResult> ObtenerCitasPorMedico(int idUsuario)
        {
            var citas = await _context.MedicoConsultorios
                .Where(mc => mc.IDUsuario == idUsuario)
                .SelectMany(mc => _context.Citas
                    .Where(c => c.IDConsultorio == mc.IDConsultorio)
                    .Include(c => c.Consultorio)
                    .Include(c => c.Usuario))
                .ToListAsync();

            var resultado = citas.Select(c => new
            {
                c.IDCita,
                c.Fecha,
                c.Hora,
                c.Estado,
                c.Observaciones,
                Consultorio = c.Consultorio != null ? c.Consultorio.Nombre : "N/A",
                Paciente = c.Usuario != null ? $"{c.Usuario.Nombre} {c.Usuario.Apellido}" : "N/A"
            });

            return Ok(resultado);
        }

        // POST: api/Citas
        [HttpPost]
        public async Task<ActionResult<Cita>> PostCita(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCita), new { id = cita.IDCita }, cita);
        }

        // PUT: api/Citas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, Cita cita)
        {
            if (id != cita.IDCita)
                return BadRequest();

            _context.Entry(cita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Citas.Any(e => e.IDCita == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPut("{id}/estado")]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] JsonElement body)
        {
            if (!body.TryGetProperty("estado", out var estadoElement))
                return BadRequest("Debe incluir el campo 'estado'.");

            var estado = estadoElement.GetString();

            var cita = await _context.Citas.FindAsync(id);
            if (cita == null) return NotFound();

            cita.Estado = estado;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estado actualizado" });
        }



        // DELETE: api/Citas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
            {
                return NotFound();
            }

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("MisCitas")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Cita>>> ObtenerCitasUsuario()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var citas = await _context.Citas
                .Include(c => c.Consultorio)
                .Where(c => c.IDUsuario == userId)
                .ToListAsync();

            return Ok(citas);
        }

        [HttpPost("solicitar")]
        public async Task<IActionResult> SolicitarCita([FromBody] Cita cita)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            cita.Estado = "Solicitada";

            Console.WriteLine($"📅 Fecha recibida: {cita.Fecha}");
            Console.WriteLine($"🕒 Hora recibida: {cita.Hora}");
            Console.WriteLine($"🏥 Consultorio ID: {cita.IDConsultorio}");

            bool citaExistente = await _context.Citas.AnyAsync(c =>
                c.IDConsultorio == cita.IDConsultorio &&
                c.Fecha.Date == cita.Fecha.Date &&
                c.Hora == cita.Hora &&
                c.Estado != "Cancelada"
            );

            if (citaExistente)
            {
                return BadRequest(new { mensaje = "Ya existe una cita agendada a esa hora en ese consultorio." });
            }

            cita.Estado = "Solicitada";
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Solicitud de cita enviada. Un médico la revisará pronto." });
        }
    }
}