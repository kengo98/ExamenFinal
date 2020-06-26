namespace ObtenerDatos
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using ExamenFinalTriggerIot.Helpers;
    using ExamenFinalTriggerIot.Models;

    public static class ObtenerDatos
    {
        [FunctionName("ObtenerDatos")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [CosmosDB(
                databaseName: Constantes.COSMOS_DB_DATABASE_NAME,
                collectionName: Constantes.COSMOS_DB_CONTAINER_NAME,
                ConnectionStringSetting = "StrCosmos",
                SqlQuery ="SELECT top 10 * FROM c order by c._ts desc")] IEnumerable<TempHum> datos,
            ILogger log)
        {
            if (datos == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(datos);
        }
    }
}