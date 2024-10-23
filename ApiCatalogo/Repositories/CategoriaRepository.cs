using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository // vai implementar a interface e herdar o rep genérico 
{
    private readonly AppDbContext _context;
    public CategoriaRepository(AppDbContext context) : base(context) // se precisar de injeção de contexto, buscar na classe base
    {
    }

    
}