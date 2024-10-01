using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiCatalogo.Models;
[Table("Categorias")] //redundante pois em DbContext já se fez esse mapeamento
public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

    [Key] //redundante pois o EF reconhece como Id
    public int CategoriaId { get; set; } //Entity framework reconhece como chave primária Id ou EntityId

    [Required]
    [StringLength(80)]
    public string? Nome { get; set;}

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set;}
    public ICollection<Produto>? Produtos { get; set; }  //relacionamento com a classe produto
}