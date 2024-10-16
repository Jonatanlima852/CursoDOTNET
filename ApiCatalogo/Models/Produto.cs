using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ApiCatalogo.Validations;

namespace ApiCatalogo.Models;

[Table("Produtos")]
public class Produto
{
    [Key]
    public int ProdutoId { get; set; } //Entity framework reconhece como chave primária Id ou EntityId
    
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(80, ErrorMessage = "O nome deve ter no máximo {1} e no mínimo {2} caracteres", MinimumLength = 5)]
    [PrimeiraLetraMaiuscula]
    public string? Nome { get; set;}
    [Required]
    [StringLength(300, ErrorMessage = "A descrição deve ter no máximo {1} caracteres")]
    public string? Descricao { get; set;}
    [Required]
    [Column(TypeName = "decimal(10, 2)")]
    [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
    public decimal Preco { get; set;}

    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set;}

    public float Estoque { get; set;}

    public DateTime DataCadastro { get; set;}

    public int CategoriaId { get; set; } //para deixar mais explícito relacionamento entre entidades
    
    [JsonIgnore] //Ignorando propriedade na serialização
    public Categoria? Categoria { get; set; } 
}

// são classes anêmicas: só possuem propriedades. Não possuem comportamento. Entity Framework fará o mapeamento para banco de dados
// Utilizaremos a abordagem code-first: realiza-se o mapeamento das entidas do domínio para o banco de dados
// O EF Core faz o mapeamento com base em algumas configurações e convenções que realizaremos. Realiza Migrations para modificar o DB.
// precisa-se instalar referências para o EF Core e o provedor para SQL Server