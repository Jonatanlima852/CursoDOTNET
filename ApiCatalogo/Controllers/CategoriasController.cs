using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCatalogo.Models;
using ApiCatalogo.Context;
using ApiCatalogo.Filters;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _repository; //utiliza a interface -> pode receber qualquer classe que implementa os métodos
        private readonly ILogger _logger;
        public CategoriasController(ICategoriaRepository repository, ILogger<CategoriasController> logger) 
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] //Utilizando filtro através do ServiceFilter
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetCategorias();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if(categoria is null)
            {
                _logger.LogWarning($"Categoria com id= {id} não encontrada...");
                return NotFound($"Categoria com id= {id} não encontrada...");
            }
            return categoria;
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if(categoria is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest($"Dados inválidos...");
            }

            var categoriaCriada = _repository.Create(categoria);

            return new CreatedAtRouteResult("ObterCAtegoria",
                new {id = categoriaCriada.CategoriaId}, categoriaCriada);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest($"Dados inválidos...");
            }

            _repository.Update(categoria);

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _repository.GetCategoria(id);

            if(categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            var categoriaExcluida = _repository.Delete(id);

            return Ok(categoriaExcluida);
        }

        // [HttpGet("produtos")]
        // public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        // {
        //     _logger.LogInformation("========= GET api/categorias/produtos ============");

        //     var categorias_com_produtos = _context.Categorias.AsNoTracking().Take(10).Include(c => c.Produtos).Where(c => c.CategoriaId <= 5).ToList();
        //     if(categorias_com_produtos is null)
        //     {
        //         return NotFound("Categorias não encontradas");
        //     }
        //     return categorias_com_produtos;

        // }

    }
}