namespace ApiCatalogo.Logging;
public class CustomLoggerProviderConfiguration 
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning; // define o nível mínimo de log a ser registrado, padrão sendo warning
    public int EventId { get; set; } = 0; //define o ID do evento de Log, padrão sendo 0
}