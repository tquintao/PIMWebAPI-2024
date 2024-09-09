namespace PIMWebAPI.Controllers
{
    // Importações de namespaces necessários
    using Microsoft.AspNetCore.Mvc;               // Para trabalhar com MVC e criar controladores
    using Microsoft.EntityFrameworkCore;          // Para interagir com o banco de dados usando o Entity Framework Core
    using PIMWebAPI.Data;                         // Referência ao contexto de dados (AppDbContext)
    using PIMWebAPI.Models;                       // Referência ao modelo Cliente
    using System.Collections.Generic;             // Para trabalhar com coleções como List
    using System.Linq;                            // Para consultas LINQ
    using System.Threading.Tasks;                 // Para métodos assíncronos

    // Define a rota base do controlador como "api/[controller]", onde [controller] será substituído pelo nome da classe "Clientes".
    [Route("api/[controller]")]
    [ApiController]  // Indica que este controlador responde a requisições de API
    public class ClientesController : ControllerBase  // ControllerBase é usado para APIs RESTful
    {
        // Dependência do contexto do banco de dados (AppDbContext), necessário para interagir com as tabelas.
        private readonly AppDbContext _context;

        // Construtor que injeta o contexto do banco de dados.
        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // Método GET: api/Clientes
        // Este método retorna uma lista de todos os clientes. É assíncrono, o que significa que não bloqueia a execução enquanto espera pelo banco de dados.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();  // Consulta assíncrona para retornar todos os clientes.
        }

        // Método GET: api/Clientes/{id}
        // Retorna um cliente específico com base no seu ID. Se o cliente não for encontrado, retorna um status 404 (NotFound).
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);  // Busca assíncrona do cliente pelo ID.

            if (cliente == null)  // Verifica se o cliente foi encontrado.
            {
                return NotFound();  // Retorna 404 se o cliente não existir.
            }

            return cliente;  // Retorna o cliente encontrado.
        }

        // Método POST: api/Clientes
        // Insere um novo cliente no banco de dados. O objeto cliente é recebido no corpo da requisição.
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);  // Adiciona o cliente ao contexto (não salva no banco ainda).
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados de forma assíncrona.

            // Retorna 201 (Created) com a URI do novo recurso e o cliente criado.
            return CreatedAtAction("GetCliente", new { id = cliente.ID_Cliente }, cliente);
        }

        // Método PUT: api/Clientes/{id}
        // Atualiza um cliente existente. Verifica se o ID passado na URL corresponde ao ID do cliente enviado no corpo da requisição.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            // Se os IDs não coincidirem, retorna um erro 400 (Bad Request).
            if (id != cliente.ID_Cliente)
            {
                return BadRequest();
            }

            // Marca o cliente como modificado no contexto (para ser atualizado no banco de dados).
            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();  // Tenta salvar as alterações de forma assíncrona.
            }
            catch (DbUpdateConcurrencyException)  // Captura exceções de concorrência, que ocorrem quando várias transações tentam modificar o mesmo recurso.
            {
                if (!ClienteExists(id))  // Verifica se o cliente ainda existe.
                {
                    return NotFound();  // Se o cliente não existir, retorna 404.
                }
                else
                {
                    throw;  // Caso contrário, relança a exceção.
                }
            }

            return NoContent();  // Retorna 204 (No Content) indicando que a atualização foi bem-sucedida, mas sem conteúdo no corpo da resposta.
        }

        // Método DELETE: api/Clientes/{id}
        // Exclui um cliente existente com base no ID fornecido.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);  // Busca o cliente pelo ID.

            if (cliente == null)  // Se o cliente não for encontrado, retorna 404.
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);  // Remove o cliente do contexto.
            await _context.SaveChangesAsync();  // Salva as mudanças de forma assíncrona.

            return NoContent();  // Retorna 204 (No Content) indicando que a exclusão foi bem-sucedida.
        }

        // Método auxiliar para verificar se um cliente existe no banco de dados.
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ID_Cliente == id);  // Verifica se algum cliente com o ID fornecido existe.
        }
    }
}
