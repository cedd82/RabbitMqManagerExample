namespace basicExampleWorkingWithRabbitMq;

internal static class MqEnvironment
{
    public const string Local = "localhost";
    public const string Uat = "uatserver";
    public const string Prod = "prodserver";
}

internal static class Queues
{
    public const string IngresErrorForReprocessing = "api/queues/%2f/manifestIngresError_ForReprocessing_Queue";
    public const string PostgresErrorForReprocessing = "api/queues/%2f/manifestPostgresError_ForReprocessing_Queue";
    public const string DuplicateEvent = "api/queues/%2f/manifestError_DuplicateEvent_Queue";
    public const string InvalidMessage = "api/queues/%2f/manifestError_InvalidMessage_Queue";
    public const string PostgresUnableToReprocessError = "api/queues/%2f/manifestPostgresError_UnableToReprocessError_Queue";
    public const string IngresUnableToReprocessError = "api/queues/%2f/manifestIngresError_UnableToReprocessError_Queue";
    public const string ManifestProcessQueue = "api/queues/%2f/manifestProcess_Queue";
}