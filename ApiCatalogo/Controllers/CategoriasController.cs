using Microsoft.AspNetCore.Mvc;
using ApiCatalogo.Models;
using ApiCatalogo.Filters;
using ApiCatalogo.Repositories;

namespace ApiCatalogo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnityOfWork _uof; //utiliza a interface -> pode receber qualquer classe que implementa os métodos
        private readonly ILogger _logger;
        public CategoriasController(IUnityOfWork uof, ILogger<CategoriasController> logger) 
        {
            _logger = logger;
            _uof = uof;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] //Utilizando filtro através do ServiceFilter
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _uof.CategoriaRepository.GetAll();
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

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

            var categoriaCriada = _uof.CategoriaRepository.Create(categoria);
            _uof.Commit();

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

            _uof.CategoriaRepository.Update(categoria);
            _uof.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

            if(categoria is null)
            {
                _logger.LogWarning($"Categoria com id={id} não encontrada...");
                return NotFound($"Categoria com id={id} não encontrada...");
            }

            var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
            _uof.Commit();

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