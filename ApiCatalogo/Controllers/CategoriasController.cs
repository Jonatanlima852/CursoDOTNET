using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCatalogo.Models;
using ApiCatalogo.Context;
using ApiCatalogo.Filters;

namespace ApiCatalogo.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger) 
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))] //Utilizando filtro através do ServiceFilter
        public async Task<ActionResult<IEnumerable<Categoria>>> Get()
        {
            try {
                return await _context.Categorias.AsNoTracking().ToListAsync();
            }
            catch (Exception) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar sua solicitação");
            }
        }

        [HttpGet("{id:int}", Name="ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
            if(categoria == null)
            {
                return NotFound("Produto não encontrado.");
            }
            return categoria;
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if(categoria == null)
                return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCAtegoria",
                new {id = categoria.CategoriaId}, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if(id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(c => c.ProdutoId == id);

            if(produto is null)
            {
                return NotFound("Produto não localizado");
            }

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            _logger.LogInformation("========= GET api/categorias/produtos ============");

            var categorias_com_produtos = _context.Categorias.AsNoTracking().Take(10).Include(c => c.Produtos).Where(c => c.CategoriaId <= 5).ToList();
            if(categorias_com_produtos is null)
            {
                return NotFound("Categorias não encontradas");
            }
            return categorias_com_produtos;

        }

    }
}