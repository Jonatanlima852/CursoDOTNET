
using ApiCatalogo.Context;
using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;
public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    //IQueryable não executa a consulta imediatamente, mas pode ser extendida ou modificada (Lazy Loading)
    // Vantagens: adicionar filtros, projeções, limites -> permite extrair menos dados de forma otimizada, 
    // por exemplo: GetProdutos().Where(p.Preco > 50) ou GetProdutos().Select(p => p.nome)
    // IQueryable é muito utilizado para realizar a paginação de dados
    public IQueryable<Produto> GetProdutos()  
    {
        return _context.Produtos;
    }

    public Produto GetProduto(int id)
    {
        return _context.Produtos.FirstOrDefault(c => c.ProdutoId == id);
    }

    public Produto Create(Produto produto)
    {
        if(produto is null)
            throw new InvalidOperationException("O produto é nulo");

        _context.Produtos.Add(produto);
        _context.SaveChanges();
        return produto;
    }

    public bool Delete(int id)
    {
        var produto = _context.Produtos.Find(id);

        if(produto is not null)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
            return true;
        }
        return false;


        
    }
    
    public bool Update(Produto produto)
    {
        if(produto is null)
            throw new InvalidOperationException("O produto é nulo");

        if (_context.Produtos.Any(p => p.ProdutoId == produto.ProdutoId)) // Any é uma função LINQ (consulta integrada à linguagem)
        {
            _context.Produtos.Update(produto); //Abordagem mais direta, mais eficiente para alterar uma entidade que já está sendo rastreada pelo contexto do EF Core. A abordagem com EntityStateModified é mais útil para cenários desconectados. Nesse cenário não importa
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}