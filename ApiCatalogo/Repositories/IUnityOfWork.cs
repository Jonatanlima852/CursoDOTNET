namespace ApiCatalogo.Repositories;

public interface IUnityOfWork 
{
    // vamos ter duas propriedades que permitem acessar os repositórios, pois ela envolve os repositórios
    IProdutoRepository ProdutoRepository { get; } // permite acessar instâncias desses repositórios
    ICategoriaRepository CategoriaRepository { get; } // Poderíamos utilizar repositórios genéricos, mas perde capacidade de implementar métodos personalizados  

    void Commit();





}