using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCatalogo.Models;
using ApiCatalogo.Context;
using Microsoft.Extensions.Logging;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository; // variável somente leitura para não ser alterado após inicializar
        private readonly IRepository<Produto> _repository; // variável somente leitura para não ser alterado após inicializar
        private readonly ILogger _logger; 
        public ProdutosController(IProdutoRepository produtoRepository, IRepository<Produto> repository, ILogger<ProdutosController> logger)  // construtor que recebe o contexto e passa para a classe
        {
            _produtoRepository = produtoRepository; // vamos utilizar o repositório especifico para um dos endpoints
            _repository = repository;
            _logger = logger;
        }

        // Para acessar um DB a recomendação é utilizar métodos assícronos
        // Usamos decorator para que o controlador receba as requisições tipo Get
        // Inumerable é otimizado, por demanda, não guarda tudo na memória 
        [HttpGet] 
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _repository.GetAll().ToList();
            return Ok(produtos);
        }

        [HttpGet("{id:int}", Name="ObterProduto")]  //para receber o id e obter na determinada rota
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _repository.Get(p => p.ProdutoId == id);            
            if(produto is null)
            {
                return NotFound("Produto não encontrado");
            }
            return Ok(produto);
        }

        // Com o decorador do API controller, não é mais necessário o decorador [FromBody] e nem verificação se é consistente com o Model
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if(produto is null)
                return BadRequest();

            var novoProduto = _repository.Create(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new {id = novoProduto.ProdutoId}, produto);       // retorna código 201 de criação 
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)  // recebe o id como parametro da url e  produto no body
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest(); //retorna 400
            }

            // como estamos em um cenário desconectado(EF Core e DB), precisamos avisar o contexto de que a entidade do produto está em um cenário modificado
            // _context.Entry(produto).State = EntityState.Modified;  //EF Core entenderá que esta entidade precisa ser persistida
            // _context.SaveChanges();

            var produtoAtualizado = _repository.Update(produto);

            return Ok(produtoAtualizado);

            //return Ok(produto);  // Status 200 e produto alterado
        } // Observe que essa abordagem exige que todos os dados sejam passados. Outra abordagem utilizaria o Patch. 


        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            // var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            // if(produto is null)
            // {
            //     _logger.LogWarning($"Produto com id={id} não encontrado...");
            //     return NotFound($"Produto com id={id} não encontrado...");
            // }
            // _context.Produtos.Remove(produto); // necessário pois estamos em cenário desconectado
            // _context.SaveChanges();

            // return Ok(produto); //StatusCode 200 e o produto excluído


            var produto = _repository.Get(p => p.ProdutoId == id);
            var produtoDeletado = _repository.Delete(produto);
            return Ok(produtoDeletado);
        }

        [HttpGet("produto/{id}")]
        public ActionResult <IEnumerable<Produto>> GetProdutosCategoria(int id)
        {
            var produtos = _produtoRepository.GetProdutosPorCategoria(id);

            if (produtos is not null)
                return NotFound();

            return Ok(produtos);    
        }
    }
}