namespace PIMWebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PIMWebAPI.Data;
    using PIMWebAPI.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    // Define a rota como "api/pedidos" para o controlador
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        // Dependência do contexto do banco de dados (DbContext)
        private readonly AppDbContext _context;

        // Construtor que injeta o contexto do banco de dados
        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        // ================================
        // GET: api/Pedidos
        // ================================
        // Retorna uma lista de todos os pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
        {
            return await _context.Pedidos.ToListAsync();  // Busca todos os pedidos no banco de dados
        }

        // ================================
        // GET: api/Pedidos/{id}
        // ================================
        // Retorna um pedido específico com base no ID fornecido
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);  // Busca o pedido pelo ID

            if (pedido == null)
            {
                return NotFound();  // Retorna 404 se o pedido não for encontrado
            }

            return pedido;  // Retorna o pedido encontrado
        }

        // ================================
        // POST: api/Pedidos
        // ================================
        // Cria um novo pedido
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);  // Adiciona o novo pedido ao DbContext
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados

            // Retorna o pedido criado com o código de status 201 (Created)
            return CreatedAtAction("GetPedido", new { id = pedido.ID_Pedido }, pedido);
        }

        // ================================
        // PUT: api/Pedidos/{id}
        // ================================
        // Atualiza um pedido existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, Pedido pedido)
        {
            if (id != pedido.ID_Pedido)
            {
                return BadRequest();  // Retorna 400 se o ID não corresponder
            }

            // Converter Data_Pedido para UTC antes de salvar no banco de dados
            pedido.Data_Pedido = DateTime.SpecifyKind(pedido.Data_Pedido, DateTimeKind.Utc);

            _context.Entry(pedido).State = EntityState.Modified;  // Marca o pedido como modificado

            try
            {
                await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound();  // Retorna 404 se o pedido não for encontrado
                }
                else
                {
                    throw;
                }
            }

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // DELETE: api/Pedidos/{id}
        // ================================
        // Exclui um pedido existente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);  // Busca o pedido pelo ID
            if (pedido == null)
            {
                return NotFound();  // Retorna 404 se o pedido não for encontrado
            }

            _context.Pedidos.Remove(pedido);  // Remove o pedido do DbContext
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // Método auxiliar para verificar se um pedido existe
        // ================================
        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.ID_Pedido == id);  // Verifica se o pedido com o ID fornecido existe no banco de dados
        }
    }
}
