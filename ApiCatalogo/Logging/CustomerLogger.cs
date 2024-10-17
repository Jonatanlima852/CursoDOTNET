

namespace ApiCatalogo.Logging;

public class CustomerLogger : ILogger
{
    // Armazena o nome do logger
    readonly string loggerName;
    // Armazena a configuração do provedor de log  
    readonly CustomLoggerProviderConfiguration loggerConfig;

    // Construtor para inicializar o logger com seu nome e configuração
    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    // Verifica se o nível de log atual está habilitado 
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    // Inicia um escopo para registrar logs, mas retorna null pois não será utilizado
    public IDisposable BeginScope<TState> (TState state)
    {
        return null;
    }

    // Método para registrrar Logs -> Verifica se o nível é permitido, formata e escreve no arquivo de logging
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string mensagem = $"{logLevel.ToString()}: {eventId} - {formatter(state, exception)}";

        EscreverTextoNoArquivo(mensagem);
    }

    private void EscreverTextoNoArquivo(string mensagem)
    {
        string caminhoArquivoLog = @"d:\dados\log\Aplicacao_dotnet_Log.txt";

        using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
        {
            try
            {
                streamWriter.WriteLine(mensagem);
                streamWriter.Close();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}