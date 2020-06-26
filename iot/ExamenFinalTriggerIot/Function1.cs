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
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExamenFinalTriggerIot.Helpers;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;

namespace ExamenFinalTriggerIot
{
    public class Function1
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("Function1")]
        public static async Task RunAsync([IoTHubTrigger("messages/events", Connection = "ConnectionString")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
            var data = JsonConvert.DeserializeObject<TempHum>(Encoding.UTF8.GetString(message.Body.Array));
            var datos = new TempHum
            {
                messageId = data.messageId,
                deviceId = data.deviceId,
                temperature = data.temperature,
                humidity = data.humidity
            };
            await Insert(datos);
        }
        private static async Task<IActionResult> Insert(TempHum a)
        {
            IActionResult returnValue = null;
            DocumentClient client;
            client = new DocumentClient(new Uri("https://proyectoparcialuno.documents.azure.com:443/"), Constantes.COSMOS_DB_PRIMMARY_KEY);
            try
            {
                var collectionUri = UriFactory.CreateDocumentCollectionUri(Constantes.COSMOS_DB_DATABASE_NAME, Constantes.COSMOS_DB_CONTAINER_NAME);
                var documentResponse = await client.CreateDocumentAsync(collectionUri, a);
                returnValue = new OkObjectResult(a);
            }
            catch (Exception ex)
            {
                returnValue = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return returnValue;
        }

    }
}