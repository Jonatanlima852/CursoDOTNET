using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;

public interface IProdutoRepository
{
    IQueryable<Produto> GetProdutos(); // IEnumerable é mais aderente ao padrão repository. IQueryable é útil para implementar paginação
    Produto GetProduto(int id);
    Produto Create(Produto produto);
    bool Update(Produto produto); //usando bool para fazer uma implementação diferente de ICategoria
    bool Delete(int id);
}