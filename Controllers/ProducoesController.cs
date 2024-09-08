using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMWebAPI.Data;
using PIMWebAPI.Models;

namespace PIMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProducoesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producao>>> GetProducoes()
        {
            return await _context.Producoes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producao>> GetProducao(int id)
        {
            var producao = await _context.Producoes.FindAsync(id);

            if (producao == null)
            {
                return NotFound();
            }

            return producao;
        }

        [HttpPost]
        public async Task<ActionResult<Producao>> PostProducao(Producao producao)
        {
            _context.Producoes.Add(producao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducao", new { id = producao.ID_Producao }, producao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducao(int id, Producao producao)
        {
            if (id != producao.ID_Producao)
            {
                return BadRequest();
            }

            // Converter Data_Producao para UTC antes de salvar no banco de dados
            producao.Data_Producao = DateTime.SpecifyKind(producao.Data_Producao, DateTimeKind.Utc);

            _context.Entry(producao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProducaoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducao(int id)
        {
            var producao = await _context.Producoes.FindAsync(id);
            if (producao == null)
            {
                return NotFound();
            }

            _context.Producoes.Remove(producao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProducaoExists(int id)
        {
            return _context.Producoes.Any(e => e.ID_Producao == id);
        }
    }

}
