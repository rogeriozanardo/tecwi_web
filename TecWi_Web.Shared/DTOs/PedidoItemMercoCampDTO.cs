using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class PedidoItemMercoCampDTO
    {
        [JsonPropertyName("NUMSEQ")]
        public string Sequencia { get; set; }

        [JsonPropertyName("CODPROD")]
        public string CodigoProduto { get; set; }

        [JsonPropertyName("QTDPROD")]
        public string Quantidade { get; set; }

        [JsonPropertyName("LOTFAB")]
        public string LoteFabricacao { get; set; }
        [JsonIgnore()]
        public decimal? Peso { get; set; }
    }
}
