using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;
    public CategoriaRepository(AppDbContext context) // injeção do contexto de mapeamento de dados
    {
        _context = context;
    }
    public Categoria Create(Categoria categoria)
    {
        if(categoria == null) throw new ArgumentNullException(nameof(categoria));

        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return categoria;
    }

    public Categoria GetCategoria(int id)
    {
        return _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
    }

    public IEnumerable<Categoria> GetCategorias()
    {
        return _context.Categorias.ToList();
    }

    public Categoria Update(Categoria categoria)
    {
        if(categoria is null) throw new ArgumentNullException(nameof(categoria));

        _context.Entry(categoria).State = EntityState.Modified; // na memória há os objetos carregados. Passa-se um com dados modificados para atualização
        _context.SaveChanges();

        return categoria;
        
    }

    public Categoria Delete(int id)
    {
        var categoria = _context.Categorias.Find(id); // buscando pelo Id utiliza-se chave primária -> busca mais eficiente

        if(categoria is null) throw new ArgumentNullException(nameof(categoria));

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return categoria;
    }
}