namespace ApiCatalogo.Models;

public class Categoria
{
    public int CategoriaId { get; set; } //Entity framework reconhece como chave primária Id ou EntityId
    public string? Nome { get; set;}
    public string? ImageUrl { get; set;}
}