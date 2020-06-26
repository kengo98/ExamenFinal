
namespace sendMessage.Models
{
    using Newtonsoft.Json;
    public class Mensaje
    {
            [JsonProperty("mensaje")]
            public string mensaje { get; set; }
    }
}
