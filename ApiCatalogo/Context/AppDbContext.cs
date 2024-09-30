using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Context;

public class AppDbContext : DbContext //classe herda DbContext: representa uma sessão com o banco de dados, sendo a ponte entre entidades e banco
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) // pega as options recebidas para configurar e passa para o construtor da classe base
    {
    }

    public DbSet<Categoria>? Categorias { get; set; } // Fazendo mapeamento utilizando DbSet(uma coleção de entidades no contexto que pode ser consultada no DB- uma linha p/ cada categoria formando a tabela Categorias)
    public DbSet<Produto>? Produtos { get; set; }
}