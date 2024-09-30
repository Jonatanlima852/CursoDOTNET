namespace ApiCatalogo.Models;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }
    public int CategoriaId { get; set; } //Entity framework reconhece como chave primária Id ou EntityId
    public string? Nome { get; set;}
    public string? ImageUrl { get; set;}
    public ICollection<Produto>? Produtos { get; set; }  //relacionamento com a classe produto
}