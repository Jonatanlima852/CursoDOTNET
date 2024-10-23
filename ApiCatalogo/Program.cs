using Microsoft.EntityFrameworkCore;
using ApiCatalogo.Context;
using System.Text.Json.Serialization;
using ApiCatalogo.Extensions;
using ApiCatalogo.Filters;
using ApiCatalogo.Logging;
using ApiCatalogo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => 
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options => 
{
    options.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

            
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
// conexao com banco através da string de conexão em appsettings.json
// Incluiu serviço do contexto do EF Core no conteiner DI nativo
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(mySqlConnection));  

builder.Services.AddScoped<ApiLoggingFilter>(); //adicionando filtro ao conteiner DI 

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>(); // registro de serviço addScoped(toda vez nova instância) - injeção de dependência da classe concreta que implementa a interface 
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>(); // sempre que for solicitada uma instância da interface, será fornecida a implementação da classe concreta
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
