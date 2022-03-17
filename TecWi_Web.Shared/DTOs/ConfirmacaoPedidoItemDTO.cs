using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class ConfirmacaoPedidoItemDTO
    {
        [JsonPropertyName("NUMSEQ")]
        public string Sequencia { get; set; }

        [JsonPropertyName("CODPROD")]
        public string CodigoProduto { get; set; }

        [JsonPropertyName("QTPROD")]
        public string Quantidade { get; set; }

        [JsonPropertyName("QTCONF")]
        public string QuantidadeConferida { get; set; }
    }
}
