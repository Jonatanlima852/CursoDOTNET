
using System.Linq.Expressions;

namespace ApiCatalogo.Repositories;

public interface IRepository<T> 
{
    // métodos comuns a serem utilizados por todas entidades, apenas

    IEnumerable<T> GetAll();

    T? Get(Expression<Func<T, bool>> predicate); // recebe uma função lambda que retorna um bool

    T Create(T entity);

    T Update(T entity);

    T Delete(T entity);
}