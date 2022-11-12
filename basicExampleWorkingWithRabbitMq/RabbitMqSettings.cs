namespace basicExampleWorkingWithRabbitMq;

public class RabbitMqSettings
{
    public string Exchange { get; set; }
    public string HostName { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public int Port { get; set; }
    public int PortSsl { get; set; }
    public string VirtualHost { get; set; }
    public string ManifestProcessQueue { get; set; }
    public string ManifestProcessQueueRoute { get; set; }
    public string ManifestPostgresErrorQueue { get; set; }
    public string ManifestPostgresErrorQueueRoute { get; set; }
    public string ManifestIngresErrorQueue { get; set; }
    public string ManifestIngresErrorQueueRoute { get; set; }
    public string ManifestPostgresErrorForReprocessingQueue { get; set; }
    public string ManifestPostgresErrorForReprocessingQueueRoute { get; set; }
    public string ManifestIngresErrorForReprocessingQueue { get; set; }
    public string ManifestIngresErrorForReprocessingQueueRoute { get; set; }
    public string ManifestErrorDuplicateProcessEventQueue { get; set; }
    public string ManifestErrorDuplicateProcessEventQueueRoute { get; set; }
    public string ManifestErrorInvalidMessageQueue { get; set; }
    public string ManifestErrorInvalidMessageQueueRoute { get; set; }
    public string ManifestPostgresErrorUnableToReprocessQueue { get; set; }
    public string ManifestPostgresErrorUnableToReprocessQueueRoute { get; set; }
    public string ManifestIngresErrorUnableToReprocessQueue { get; set; }
    public string ManifestIngresErrorUnableToReprocessQueueRoute { get; set; }
    public int ManifestIngresErrorQueueRetryCount { get; set; }
    public int ManifestPostgresErrorQueueRetryCount { get; set; }
    public bool UseQueue { get; set; }
    public string AppProducingMessage { get; set; }
}