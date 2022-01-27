using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class EmbalagemMercoCampDTO
    {
        [JsonPropertyName("CODUNID")]
        public string UnidadeVenda { get; set; }
        [JsonPropertyName("IEMB_ENT")]
        public string PadraoEntrada { get; set; }
        [JsonPropertyName("IEMB_SAI")]
        public string PadraoSaida { get; set; }
        [JsonPropertyName("CODBARRA")]
        public string CodigoBarra { get; set; }
       
    }
}
