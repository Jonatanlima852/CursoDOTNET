using ApiCatalogo.Context;
using ApiCatalogo.Models;

namespace ApiCatalogo.Repositories;

public class UnityOfWork : IUnityOfWork
{
    private IProdutoRepository _produtoRepository;
    private ICategoriaRepository _categoriaRepository;
    public AppDbContext _context;

    public UnityOfWork(AppDbContext context)
    {
        _context = context;
    }

    // Esse código retorna uma instância para o repositório (através do get). Mas, para isso, ele verifica se já possui o atributo, se não, cria um novo com o contexto que recebeu
    // Assim evita a criação de instâncias desnecessárias de repositório
    public IProdutoRepository ProdutoRepository
    {
        get
        {
            return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context); //observe que a instãncia do contexto deve ser pública, para estar disponnível para ser injetada
        }
    }

    // Poderíamos colocar no construtor, mas isso garante que vamos ter as instâncias apenas se não existirem e se precisarmos
    // É uma abordagem Lazy Loading, para evitar criação de objetos desnecessários
    // Vamos registrar como AddScopped, ou seja, vamos ter apenas uma instância para cada requisição http
    public ICategoriaRepository CategoriaRepository
    {
        get
        {
            return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context); 
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    // classe para liberar recursos alocados para este contexto(ex. conexões com o DB)
    // O sistema já faz isso automaticamente, mas vamos garantir a liberação mesmo para recursos não gerenciados. 
    public void Dispose()
    {
        _context.Dispose();
    }
}