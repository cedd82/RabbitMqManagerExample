using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace basicExampleWorkingWithRabbitMq;

internal class PostMessagesToQueues
{
    private readonly Dictionary<string, string> _queueRoutes;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private IModel _channel;
    private IConnection _connection;
    private ConnectionFactory _factory;

    public PostMessagesToQueues(RabbitMqSettings rabbitMqSettings)
    {
        _rabbitMqSettings = rabbitMqSettings;
        _queueRoutes = new Dictionary<string, string>
        {
            { rabbitMqSettings.ManifestProcessQueue, rabbitMqSettings.ManifestProcessQueueRoute },
            { rabbitMqSettings.ManifestPostgresErrorQueue, rabbitMqSettings.ManifestPostgresErrorQueueRoute },
            { rabbitMqSettings.ManifestIngresErrorQueue, rabbitMqSettings.ManifestIngresErrorQueueRoute },
            { rabbitMqSettings.ManifestPostgresErrorForReprocessingQueue, rabbitMqSettings.ManifestPostgresErrorForReprocessingQueueRoute },
            { rabbitMqSettings.ManifestIngresErrorForReprocessingQueue, rabbitMqSettings.ManifestIngresErrorForReprocessingQueueRoute },
            { rabbitMqSettings.ManifestErrorDuplicateProcessEventQueue, rabbitMqSettings.ManifestErrorDuplicateProcessEventQueueRoute },
            { rabbitMqSettings.ManifestErrorInvalidMessageQueue, rabbitMqSettings.ManifestErrorInvalidMessageQueueRoute },
            { rabbitMqSettings.ManifestPostgresErrorUnableToReprocessQueue, rabbitMqSettings.ManifestPostgresErrorUnableToReprocessQueueRoute },
            { rabbitMqSettings.ManifestIngresErrorUnableToReprocessQueue, rabbitMqSettings.ManifestIngresErrorUnableToReprocessQueueRoute }
        };
    }

    internal void PostManifestMessagesFromDirectoryToQueue(string dir, string environment, string queue)
    {
        var files = Directory.GetFiles(dir);
        if (!files.Any())
        {
            Console.WriteLine("no files to send");
            return;
        }

        var queueRoute = _queueRoutes[queue];
        CreateConnection(environment, queue, queueRoute);
        var itemCount = 0;
        foreach (var file in files)
        {
            var json = File.ReadAllText(file);
            //var obj  =JsonConvert.DeserializeObject(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, });
            //Console.WriteLine(obj.GetType().FullName);
            //string json = JsonConvert.SerializeObject(obj);
            var manifest = JsonConvert.DeserializeObject<QueueMessageTyped>(json);

            //string json = JsonConvert.SerializeObject(obj);
            var bytes = Encoding.ASCII.GetBytes(json);
            SendMessage(bytes, queueRoute);
            itemCount++;
        }

        Console.WriteLine($"Sent {itemCount} files to queue: {queue} Environment:{environment}");

        Close();
    }

    private void Close()
    {
        _connection.Close();
    }

    private void CreateConnection(string environment, string queue, string queueRoute)
    {
        _factory = new ConnectionFactory
        {
            HostName = environment,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password
        };

        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(_rabbitMqSettings.Exchange, ExchangeType.Topic, true);
        _channel.QueueDeclare(queue, true, false, false, null);
        _channel.QueueBind(queue, _rabbitMqSettings.Exchange, queueRoute);
        _channel.BasicQos(0, 1, false);
        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        _connection.CallbackException += RabbitMQ_CallbackException;
    }

    private void RabbitMQ_CallbackException(object sender, CallbackExceptionEventArgs e)
    {
        Console.WriteLine($"RabbitMQ_CallbackException {e}");
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        Console.WriteLine($"  connection shut down {e.ReplyText}");
        if (!_channel.IsClosed)
        {
            Console.WriteLine("  closing rabbit mq channel");
            _channel.Close();
        }

        if (_connection.IsOpen)
        {
            Console.WriteLine("  closing rabbit mq connection");
            _connection.Close();
        }
    }

    private void SendMessage(byte[] message, string routingKey)
    {
        var basicProperties = _channel.CreateBasicProperties();
        //basicProperties.Persistent   = true;
        basicProperties.DeliveryMode = 2;
        _channel.BasicPublish(_rabbitMqSettings.Exchange, routingKey, basicProperties, message);
    }
}