using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIMWebAPI.Data;
using PIMWebAPI.Models;

namespace PIMWebAPI.Controllers
{
    // Define a rota para o controlador como "api/itempedidos"
    [Route("api/[controller]")]
    [ApiController]  // Marca a classe como um controlador de API
    public class ItemPedidosController : ControllerBase
    {
        // Referência ao contexto do banco de dados (DbContext)
        private readonly AppDbContext _context;

        // Construtor do controlador que injeta o contexto do banco de dados
        public ItemPedidosController(AppDbContext context)
        {
            _context = context;
        }

        // ================================
        // GET: api/ItemPedidos
        // ================================
        // Este método retorna uma lista de todos os itens pedidos.
        // Ele busca todos os registros da tabela "ItensPedido" e retorna em formato de lista.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemPedido>>> GetItemPedidos()
        {
            return await _context.ItensPedido.ToListAsync();  // Busca todos os itens pedidos no banco de dados
        }

        // ================================
        // GET: api/ItemPedidos/{id}
        // ================================
        // Este método retorna um item pedido específico com base no ID fornecido.
        // Se o item não for encontrado, retorna um código de status 404 (Not Found).
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemPedido>> GetItemPedido(int id)
        {
            var itemPedido = await _context.ItensPedido.FindAsync(id);  // Busca o item pedido pelo ID

            if (itemPedido == null)
            {
                return NotFound();  // Retorna 404 se o item pedido não for encontrado
            }

            return itemPedido;  // Retorna o item pedido
        }

        // ================================
        // POST: api/ItemPedidos
        // ================================
        // Este método cria um novo item pedido.
        // Ele adiciona o item ao banco de dados e retorna o item criado junto com o código de status 201 (Created).
        [HttpPost]
        public async Task<ActionResult<ItemPedido>> PostItemPedido(ItemPedido itemPedido)
        {
            _context.ItensPedido.Add(itemPedido);  // Adiciona o novo item pedido ao DbContext
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados

            // Retorna o item criado com a localização (GetItemPedido) e o ID
            return CreatedAtAction("GetItemPedido", new { id = itemPedido.ID_ItemPedido }, itemPedido);
        }

        // ================================
        // PUT: api/ItemPedidos/{id}
        // ================================
        // Este método atualiza um item pedido existente.
        // Ele verifica se o ID fornecido corresponde ao ID do item pedido que está sendo atualizado.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemPedido(int id, ItemPedido itemPedido)
        {
            if (id != itemPedido.ID_ItemPedido)
            {
                return BadRequest();  // Retorna 400 se o ID não corresponder
            }

            _context.Entry(itemPedido).State = EntityState.Modified;  // Marca o item pedido como modificado no DbContext

            try
            {
                await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemPedidoExists(id))
                {
                    return NotFound();  // Retorna 404 se o item não for encontrado
                }
                else
                {
                    throw;
                }
            }

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // DELETE: api/ItemPedidos/{id}
        // ================================
        // Este método exclui um item pedido existente.
        // Se o item pedido não for encontrado, retorna 404 (Not Found).
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemPedido(int id)
        {
            var itemPedido = await _context.ItensPedido.FindAsync(id);  // Busca o item pedido pelo ID
            if (itemPedido == null)
            {
                return NotFound();  // Retorna 404 se o item não for encontrado
            }

            _context.ItensPedido.Remove(itemPedido);  // Remove o item pedido do DbContext
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // Método auxiliar para verificar se um item pedido existe
        // ================================
        // Este método verifica se um item pedido com o ID fornecido existe no banco de dados.
        private bool ItemPedidoExists(int id)
        {
            return _context.ItensPedido.Any(e => e.ID_ItemPedido == id);
        }
    }
}
