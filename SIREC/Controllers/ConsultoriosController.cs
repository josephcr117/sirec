using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIREC.Data;
using SIREC.Models;

namespace SIREC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultoriosController : ControllerBase
    {
        private readonly SirecDbContext _context;

        public ConsultoriosController(SirecDbContext context)
        {
            _context = context;
        }

        // GET: api/Consultorios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consultorio>>> GetConsultorios()
        {
            var consultorios = await _context.Consultorios.ToListAsync();
            return Ok(consultorios);
        }

        // GET: api/Consultorios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Consultorio>> GetConsultorio(int id)
        {
            var consultorio = await _context.Consultorios.FindAsync(id);

            if (consultorio == null)
                return NotFound();

            return consultorio;
        }

        // POST: api/Consultorios
        [HttpPost]
        public async Task<ActionResult<Consultorio>> PostConsultorio(Consultorio consultorio)
        {
            _context.Consultorios.Add(consultorio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetConsultorio), new { id = consultorio.IDConsultorio }, consultorio);
        }

        // PUT: api/Consultorios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsultorio(int id, Consultorio consultorio)
        {
            if (id != consultorio.IDConsultorio)
                return BadRequest();

            _context.Entry(consultorio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Consultorios.Any(c => c.IDConsultorio == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Consultorios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultorio(int id)
        {
            var consultorio = await _context.Consultorios.FindAsync(id);
            if (consultorio == null)
                return NotFound();

            _context.Consultorios.Remove(consultorio);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}