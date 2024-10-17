using System.Collections.Concurrent; //Importa um dicionário concorrente para armazenamento thread-safe de loggers

namespace ApiCatalogo.Logging; // define o nasmespace


// classe que implementa interface para obter logs personalizados
// cada logger é armazenado em um ConcurrentDictionary para garantir acesso seguro em um ambiente com múltiplas trheads.
// Quando o createLogger é chamado, ele retorna um logger existente para cada categoria especificada ou cria um novo
public class CustomLoggerProvider : ILoggerProvider 
{
    readonly CustomLoggerProviderConfiguration loggerConfig; //armazena configurações do logger

    // armazena instâncias de loggers com acesso thread-safe
    readonly ConcurrentDictionary<string, CustomerLogger> loggers =
                                    new ConcurrentDictionary<string, CustomerLogger>(); 

    public CustomLoggerProvider(CustomLoggerProviderConfiguration config)
    {
        loggerConfig = config;
    }

    // retorna um logger existente ou cria um logger para uma categoria especificada 
    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, loggerConfig));
    }

    // libera recursos ao limpar todos os loggers
    public void Dispose()
    {
        loggers.Clear();
    }
}