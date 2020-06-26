namespace sendMessage
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Microsoft.Azure.Devices;
    using System.Text;
    using sendMessage.Models;

    public static class SendMessage
    {
        static ServiceClient _serviceClient;
        [FunctionName("SendMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            IActionResult returnvalue = null;
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Mensaje>(requestBody);

                var mensaje = new Mensaje { mensaje = data.mensaje };


                Console.WriteLine("Send Cloud-to-Device message\n");

                string variable = Environment.GetEnvironmentVariable("StrIot");
                _serviceClient = ServiceClient.CreateFromConnectionString(variable);

                SendCloudToDeviceMessageAsync(mensaje.mensaje).Wait();

                returnvalue = new OkObjectResult(mensaje);
            }
            catch (Exception ex)
            {
                log.LogError($"Could not Send Message. Exception: {ex.Message}");
                returnvalue = new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return returnvalue;
        }
        private async static Task SendCloudToDeviceMessageAsync(string mensaje)
        {
            var commandMessage = new
             Message(Encoding.ASCII.GetBytes(mensaje));
            await _serviceClient.SendAsync("sensorTemperaturaKengo", commandMessage);

        }
    }
}