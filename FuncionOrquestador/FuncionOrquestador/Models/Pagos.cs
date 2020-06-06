namespace FuncionOrquestador.Models
{
    using Newtonsoft.Json;
    public class Pagos
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("idCliente")]
        public string idCliente { get; set; }

        [JsonProperty("productos")]
        public string[] productos { get; set; }

        [JsonProperty("fecha")]
        public string fecha { get; set; }

        [JsonProperty("montoTotal")]
        public double montoTotal { get; set; }

        [JsonProperty("pagado")]
        public bool pagado { get; set; }

    }
}
