using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCatalogo.Models;
using ApiCatalogo.Context;

namespace ApiCatalogo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context; // variável somente leitura para não ser alterado após inicializar
        public ProdutosController(AppDbContext context)  // construtor que recebe o contexto e passa para a classe
        {
            _context = context;
        }

        // Para acessar um DB a recomendação é utilizar métodos assícronos
        // Usamos decorator para que o controlador receba as requisições tipo Get
        // Inumerable é otimizado, por demanda, não guarda tudo na memória 
        [HttpGet] 
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.ToList();
            if(produtos is null)
            {
                return NotFound("Produtos não encontrados.");
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name="ObterProduto")]  //para receber o id e obter na determinada rota
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id); // vai receber o primeiro produto que satisfaz o critério
            if(produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return produto;
        }

        // Com o decorador do API controller, não é mais necessário o decorador [FromBody] e nem verificação se é consistente com o Model
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if(produto is null)
                return BadRequest();

            _context.Produtos.Add(produto); 
            _context.SaveChanges(); // Para persistir os dados na tabela

            return new CreatedAtRouteResult("ObterProduto",
                new {id = produto.ProdutoId}, produto);       // retorna código 201 de criação 
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)  // recebe o id como parametro da url e  produto no body
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest(); //retorna 400
            }

            // como estamos em um cenário desconectado(EF Core e DB), precisamos avisar o contexto de que a entidade do produto está em um cenário modificado
            _context.Entry(produto).State = EntityState.Modified;  //EF Core entenderá que esta entidade precisa ser persistida
            _context.SaveChanges();

            return Ok(produto);  // Status 200 e produto alterado
        } // Observe que essa abordagem exige que todos os dados sejam passados. Outra abordagem utilizaria o Patch. 


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não localizado...");
            }
            _context.Produtos.Remove(produto); // necessário pois estamos em cenário desconectado
            _context.SaveChanges();

            return Ok(produto); //StatusCode 200 e o produto excluído
        }
    }
}