using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class PedidoItemMercoCampDTO
    {
        [JsonPropertyName("NUMSEQ")]
        public int Sequencia { get; set; }

        [JsonPropertyName("CODPROD")]
        public string CodigoProduto { get; set; }

        [JsonPropertyName("QTDPROD")]
        public int Quantidade { get; set; }

        [JsonPropertyName("LOTFAB")]
        public string LoteFabricacao { get; set; }
    }
}
