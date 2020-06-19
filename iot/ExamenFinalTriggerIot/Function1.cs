using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json;
using ExamenFinalTriggerIot.Models;
using Microsoft.Azure.Amqp.Framing;
using System;

namespace ExamenFinalTriggerIot
{
    public static class Function1
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("Function1")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "ConnectionString")]EventData message, ILogger log)
        {
            //log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
            var data = JsonConvert.DeserializeObject<TempHum>(Encoding.UTF8.GetString(message.Body.Array));
            var datos = new TempHum
            {
                messageId = data.messageId,
                deviceId = data.deviceId,
                temperature = data.temperature,
                humidity = data.humidity
            };





        }
    }
}