

using System.Linq.Expressions;
using ApiCatalogo.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Repositories;

public class Repository<T> : IRepository<T> where T : class //garante que T herda de uma classe
{
    protected readonly AppDbContext _context;
    // protected significa que poderá ser utilizada por classes derivadas, ao contrário do private. 

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking().ToList(); // método set utilizado para acessar o conjunto correspondente à tabela T
        // adicionando as no tracking para ganhar desempenho e não rastrear entidades
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate); //fazendo uma consulta no banco que atenda ao predicado 
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public T Update(T entity)
    {
        // _context.Entry(entity).State = EntityState.Modified; -> método mais prolixo
        _context.Set<T>().Update(entity);
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }

}