using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace basicExampleWorkingWithRabbitMq;

internal class GetMessagesFromQueues
{
    private readonly RabbitMqSettings _rabbitMqSettings;

    public GetMessagesFromQueues(RabbitMqSettings rabbitMqSettings)
    {
        _rabbitMqSettings = rabbitMqSettings;
    }

    /// <summary>
    ///     This class is used to parse messages rabbitmq using curl cmds
    ///     saves files to C:\rabbitMsgs\{env}\{timestamp}\queue(s)
    ///     runs the below curl cmd to dl messages from the server, it will requeue them because"ackmode":"ack_requeue_true,
    ///     careful using this...
    ///     it will save them to a file you specified in the -o option in this case -o
    ///     manifestPostgresError_ForReprocessing_QueueUAT.json for example
    ///     this
    ///     uat:
    ///     curl -u username:pw -H "content-type:application/json" -X POST
    ///     http://YourRabbitMqServer:15672/api/queues/%2f/manifestPostgresError_ForReprocessing_Queue/get
    ///     -d'{"count":500000,"requeue":true,"encoding":"auto","truncate":50000, "ackmode":"ack_requeue_true"}' -o
    ///     manifestPostgresError_ForReprocessing_QueueUAT.json
    ///     curl -u username:pw -H "content-type:application/json" -X POST
    ///     http://YourRabbitMqServer:15672/api/queues/%2f/manifestIngresError_ForReprocessing_Queue/get
    ///     -d'{"count":500000,"requeue":true,"encoding":"auto","truncate":50000, "ackmode":"ack_requeue_true"}' -o
    ///     manifestIngresError_ForReprocessing_QueueUAT.json
    /// </summary>
    /// <param name="args"></param>
    internal async Task GetMessages(Dictionary<string, string> environments, Dictionary<string, string> queues)
    {
        var now = DateTime.Now;
        foreach (var environment in environments)
        {
            var msgCount = 0;
            var host = $"http://{environment.Value}:15672/";
            var msgOutputDir = $@"C:\rabbitMsgs\{environment.Key}";
            var subMsgOutputDir = $@"{msgOutputDir}\{now:yyyyMMddhhmmss}";

            foreach (var queue in queues)
            {
                var queueUrl = $"{host}{queue.Value}/get";
                Console.WriteLine(queueUrl);
                var response = await ExecuteRabbitCurlCmdAsync(queueUrl);
                msgCount += ProcessResponse(environment.Key, queue.Key, subMsgOutputDir, response);
            }

            if (msgCount != 0)
                Process.Start("explorer.exe", msgOutputDir);
        }
    }

    private async Task<string> ExecuteRabbitCurlCmdAsync(string queue)
    {
        using HttpClient httpClient = new();
        using HttpRequestMessage request = new(new HttpMethod("POST"), queue);
        var base64Authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_rabbitMqSettings.UserName}:{_rabbitMqSettings.Password}"));
        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64Authorization}");

        request.Content = new StringContent("{\"count\":5000,\"requeue\":true,\"encoding\":\"auto\",\"truncate\":50000000, \"ackmode\":\"ack_requeue_true\"}");
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        var response = await httpClient.SendAsync(request);
        var res = await response.Content.ReadAsStringAsync();

        return res;
    }

    private int ProcessResponse(string env, string queue, string subMsgOutputDir, string response)
    {
        var jArray = JArray.Parse(response);
        if (!jArray.HasValues)
        {
            Console.WriteLine($"env {env} no msgs on queue {queue}");
            return 0;
        }

        var msgs = jArray.Children()["payload"].ToList();

        if (!msgs.Any())
            return 0;

        var dirFlat = $@"{subMsgOutputDir}\{queue}Flat\";
        var dirFormatted = $@"{subMsgOutputDir}\{queue}\";
        Directory.CreateDirectory(dirFlat);
        Directory.CreateDirectory(dirFormatted);

        var i = 1;

        var distinctMsgs = msgs.Distinct().ToList();
        var msgCount = distinctMsgs.Count;
        foreach (var jToken in distinctMsgs)
        {
            var token = jToken;
            var manifestProcess = JsonConvert.DeserializeObject<QueueMessageTyped>(token.ToString());

            int? bookingNumber = manifestProcess?.VehicleProcessData?.Detail.BookingNumber ?? 0;
            int? bookingVehicleNumber = manifestProcess?.VehicleProcessData?.Detail.BookingVehicleNumber ?? 0;
            var manifestId = manifestProcess.ManifestId;
            var eventCode = manifestProcess.EventCode;
            var processDate = manifestProcess.ProcessDate;
            var processReference = manifestProcess.ProcessReference;
            var processDateShort = processDate.Replace(":", "").Replace("-", "").Replace('T', '-');

            //var str = x.ToString(Formatting.Indented);
            var fileFlat = $@"{dirFlat}{i}-{manifestId}-{eventCode}-{processDateShort}-({bookingNumber}-{bookingVehicleNumber})-{processReference}.json";
            var fileFormatted = $@"{dirFormatted}{i}-{manifestId}-{eventCode}-{processDateShort}-({bookingNumber}-{bookingVehicleNumber})-{processReference}.json";
            using (StreamWriter writer = new(fileFormatted))
            {
                var jsonFormatted = JsonConvert.SerializeObject(manifestProcess, Formatting.Indented);
                writer.Write(jsonFormatted);
                writer.Flush();
            }

            using (StreamWriter writer = new(fileFlat))
            {
                var jsonFlat = JsonConvert.SerializeObject(manifestProcess);
                writer.Write(jsonFlat);
                writer.Flush();
            }

            i++;
        }

        Console.WriteLine($"downloaded {msgCount} msgs from Queue:{queue} Environment:{env} savedMsgsTo:{subMsgOutputDir}");
        return msgCount;
    }
}