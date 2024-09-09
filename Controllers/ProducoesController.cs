using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMWebAPI.Data;
using PIMWebAPI.Models;

namespace PIMWebAPI.Controllers
{
    // Define a rota base "api/producoes" para esse controlador
    [Route("api/[controller]")]
    [ApiController]
    public class ProducoesController : ControllerBase
    {
        // Dependência do AppDbContext para acessar o banco de dados
        private readonly AppDbContext _context;

        // Construtor que recebe o contexto do banco de dados via injeção de dependência
        public ProducoesController(AppDbContext context)
        {
            _context = context;
        }

        // ================================
        // GET: api/Producoes
        // ================================
        // Retorna uma lista de todas as produções no banco de dados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producao>>> GetProducoes()
        {
            return await _context.Producoes.ToListAsync();  // Busca todas as produções
        }

        // ================================
        // GET: api/Producoes/{id}
        // ================================
        // Retorna uma produção específica pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Producao>> GetProducao(int id)
        {
            var producao = await _context.Producoes.FindAsync(id);  // Busca a produção pelo ID

            if (producao == null)
            {
                return NotFound();  // Retorna 404 se não encontrar
            }

            return producao;  // Retorna a produção encontrada
        }

        // ================================
        // POST: api/Producoes
        // ================================
        // Cria uma nova produção
        [HttpPost]
        public async Task<ActionResult<Producao>> PostProducao(Producao producao)
        {
            _context.Producoes.Add(producao);  // Adiciona a nova produção ao DbContext
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados

            // Retorna a produção criada com o código de status 201 (Created)
            return CreatedAtAction("GetProducao", new { id = producao.ID_Producao }, producao);
        }

        // ================================
        // PUT: api/Producoes/{id}
        // ================================
        // Atualiza uma produção existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducao(int id, Producao producao)
        {
            if (id != producao.ID_Producao)
            {
                return BadRequest();  // Retorna 400 se o ID não corresponder
            }

            // Converter Data_Producao para UTC antes de salvar no banco de dados
            producao.Data_Producao = DateTime.SpecifyKind(producao.Data_Producao, DateTimeKind.Utc);

            _context.Entry(producao).State = EntityState.Modified;  // Marca a produção como modificada

            try
            {
                await _context.SaveChangesAsync();  // Salva as alterações no banco de dados
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProducaoExists(id))
                {
                    return NotFound();  // Retorna 404 se a produção não for encontrada
                }
                else
                {
                    throw;
                }
            }

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // DELETE: api/Producoes/{id}
        // ================================
        // Exclui uma produção existente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducao(int id)
        {
            var producao = await _context.Producoes.FindAsync(id);  // Busca a produção pelo ID
            if (producao == null)
            {
                return NotFound();  // Retorna 404 se não encontrar
            }

            _context.Producoes.Remove(producao);  // Remove a produção do DbContext
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // Método auxiliar para verificar se uma produção existe
        // ================================
        private bool ProducaoExists(int id)
        {
            return _context.Producoes.Any(e => e.ID_Producao == id);  // Verifica se a produção com o ID fornecido existe
        }
    }
}
