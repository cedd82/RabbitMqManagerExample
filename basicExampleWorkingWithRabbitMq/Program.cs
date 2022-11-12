using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace basicExampleWorkingWithRabbitMq;

internal class Program
{
    private static ServiceProvider _serviceProvider;
    private static RabbitMqSettings _rabbitMqSettings;

    private static async Task Main(string[] args)
    {
        SetupServices();
        var getMessagesFromQueues = _serviceProvider.GetService<GetMessagesFromQueues>();

        // comment out what env ur dealing with
        Dictionary<string, string> environments = new()
        {
            //{"Uat", MqEnvironment.Uat},
            { "Prod", MqEnvironment.Prod }
            //{"Local", MqEnvironment.Local},
        };

        // get data from what queues you want
        Dictionary<string, string> queues = new()
        {
            { "IngresErrorForReprocessing", $"{Queues.IngresErrorForReprocessing}" },
            { "PostgresErrorForReprocessing", $"{Queues.PostgresErrorForReprocessing}" },
            { "IngresUnableToReprocessError", $"{Queues.IngresUnableToReprocessError}" },
            { "PostgresUnableToReprocessError", $"{Queues.PostgresUnableToReprocessError}" },
            //{"DuplicateEvent", $"{Queues.DuplicateEvent}"},
            { "InvalidMessage", $"{Queues.InvalidMessage}" }
            //{"manifestProcess_Queue", $"{Queues.Main}" },
        };


        //Note uncomment what u want to do

        // get message from a queue, dump them to a folder, 1 file per message
        await getMessagesFromQueues.GetMessages(environments, queues);
        
        // push messages onto a queue
        //SendMessagesToQueue(@"C:\rabbitMsgs\Prod\20211005024324\IngresErrorForReprocessing", MqEnvironment.Local, _rabbitMqSettings.ManifestIngresErrorQueue);
        //PostTestMessagesToRabbitMqServer(MqEnvironment.Local, _rabbitMqSettings.ManifestPostgresErrorQueue);

        Console.WriteLine("Done");
        Console.ReadKey();
    }

    private static void PostTestMessagesToRabbitMqServer(string environment, string queue)
    {
        var msgPoster = _serviceProvider.GetService<PostMessagesToQueues>();
        var dirWhereMsgsStored = @"C:\rabbitMsgsToSend";
        var files = Directory.GetFiles(dirWhereMsgsStored);

        foreach (var file in files)
        {
            var json = File.ReadAllText(file);
            var manifestProcess = JsonConvert.DeserializeObject<QueueMessageTyped>(json);

            // example hack fix i had to do to fix msgs on queue
            //if (manifestProcess.VehicleProcessData?.Detail?.VehicleId == "BH01TU")
            //{
            //    manifestProcess.VehicleProcessData.Detail.VehicleId = "AJM0KF2W7A00157095";
            //    manifestProcess.VehicleProcessData.Detail.BookingNumber = 1896387;
            //    manifestProcess.VehicleProcessData.Detail.BookingVehicleNumber = 1;
            //}

            manifestProcess.ProcessReference = Guid.NewGuid().ToString();
            manifestProcess.ProcessDate = DateTime.Now.ToString("s");

            json = JsonConvert.SerializeObject(manifestProcess, Formatting.Indented);
            File.WriteAllText(file, json);
        }

        msgPoster.PostManifestMessagesFromDirectoryToQueue(dirWhereMsgsStored, environment, queue);
    }

    private static void SendMessagesToQueue(string dirWhereMsgsStored, string environment, string queue)
    {
        var msgPoster = _serviceProvider.GetService<PostMessagesToQueues>();
        msgPoster.PostManifestMessagesFromDirectoryToQueue(dirWhereMsgsStored, environment, queue);
    }

    private static void SetupServices()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var configuration = builder.Build();
        IServiceCollection services = new ServiceCollection();
        _rabbitMqSettings = configuration.GetSection("rabbitMqSettings").Get<RabbitMqSettings>();
        services.AddSingleton(_rabbitMqSettings);
        services.AddTransient<GetMessagesFromQueues>();
        services.AddTransient<PostMessagesToQueues>();
        _serviceProvider = services.BuildServiceProvider();
    }
}