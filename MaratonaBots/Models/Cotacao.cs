using Newtonsoft.Json;

namespace MaratonaBots.Models
{

    public class Cotacao
    {
        [JsonProperty(PropertyName = "nome")]
        public string Nome { get; set; }

        [JsonProperty(PropertyName = "sigla")]
        public string Sigla { get; set; }

        [JsonProperty(PropertyName = "valor")]
        public float Valor { get; set; }
    }

}