namespace PIMWebAPI.Controllers
{
    using Microsoft.AspNetCore.JsonPatch;  // Necessário para suportar operações de PATCH
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PIMWebAPI.Data;
    using PIMWebAPI.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    // Define a rota base "api/produtos" para esse controlador
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        // Dependência do AppDbContext para acessar o banco de dados
        private readonly AppDbContext _context;

        // Construtor que recebe o contexto do banco de dados via injeção de dependência
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        // ================================
        // GET: api/Produtos
        // ================================
        // Retorna uma lista de todos os produtos no banco de dados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            return await _context.Produtos.ToListAsync();  // Busca todos os produtos
        }

        // ================================
        // GET: api/Produtos/{id}
        // ================================
        // Retorna um produto específico pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);  // Busca o produto pelo ID

            if (produto == null)
            {
                return NotFound();  // Retorna 404 se o produto não for encontrado
            }

            return produto;  // Retorna o produto encontrado
        }

        // ================================
        // POST: api/Produtos
        // ================================
        // Cria um novo produto
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            _context.Produtos.Add(produto);  // Adiciona o novo produto ao DbContext
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados

            // Retorna o produto criado com o código de status 201 (Created)
            return CreatedAtAction("GetProduto", new { id = produto.ID_Produto }, produto);
        }

        // ================================
        // PUT: api/Produtos/{id}
        // ================================
        // Atualiza um produto existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.ID_Produto)
            {
                return BadRequest();  // Retorna 400 se o ID não corresponder
            }

            _context.Entry(produto).State = EntityState.Modified;  // Marca o produto como modificado

            try
            {
                await _context.SaveChangesAsync();  // Salva as alterações no banco de dados
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();  // Retorna 404 se o produto não for encontrado
                }
                else
                {
                    throw;
                }
            }

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // PATCH: api/Produtos/{id}
        // ================================
        // Atualiza parcialmente um produto existente usando JSON Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduto(int id, [FromBody] JsonPatchDocument<Produto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();  // Retorna 400 se o patchDoc for nulo
            }

            var produto = await _context.Produtos.FindAsync(id);  // Busca o produto pelo ID
            if (produto == null)
            {
                return NotFound();  // Retorna 404 se o produto não for encontrado
            }

            // Aplica as alterações parciais ao produto
            patchDoc.ApplyTo(produto, ModelState);

            // Valida o modelo após a aplicação das alterações
            if (!TryValidateModel(produto))
            {
                return BadRequest(ModelState);  // Retorna 400 se a validação falhar
            }

            try
            {
                await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();  // Retorna 404 se o produto não for encontrado
                }
                else
                {
                    throw;
                }
            }

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // DELETE: api/Produtos/{id}
        // ================================
        // Exclui um produto existente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);  // Busca o produto pelo ID
            if (produto == null)
            {
                return NotFound();  // Retorna 404 se o produto não for encontrado
            }

            _context.Produtos.Remove(produto);  // Remove o produto do DbContext
            await _context.SaveChangesAsync();  // Salva as mudanças no banco de dados

            return NoContent();  // Retorna 204 (No Content) se a operação for bem-sucedida
        }

        // ================================
        // Método auxiliar para verificar se um produto existe
        // ================================
        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.ID_Produto == id);  // Verifica se o produto com o ID fornecido existe
        }
    }
}
