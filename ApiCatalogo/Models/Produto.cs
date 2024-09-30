namespace ApiCatalogo.Models;

public class Produto
{
    public int ProdutoId { get; set; } //Entity framework reconhece como chave primária Id ou EntityId
    public string? Nome { get; set;}
    public string? Descricao { get; set;}
    public decimal Preco { get; set;}
    public string? ImageUrl { get; set;}
    public float Estoque { get; set;}
    public DateTime DataCadastro { get; set;}
    public int CategoriaId { get; set; } //para deixar mais explícito relacionamento entre entidades
    public Categoria? Categoria { get; set; } 
}

// são classes anêmicas: só possuem propriedades. Não possuem comportamento. Entity Framework fará o mapeamento para banco de dados
// Utilizaremos a abordagem code-first: realiza-se o mapeamento das entidas do domínio para o banco de dados
// O EF Core faz o mapeamento com base em algumas configurações e convenções que realizaremos. Realiza Migrations para modificar o DB.
// precisa-se instalar referências para o EF Core e o provedor para SQL Server