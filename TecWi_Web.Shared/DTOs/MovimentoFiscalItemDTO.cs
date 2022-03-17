using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class MovimentoFiscalItemDTO
    {
        [JsonPropertyName("NUMSEQ")]
        public string Sequencia { get; set; }
        [JsonPropertyName("CODPROD")]
        public string CodigoProduto { get; set; }
        [JsonPropertyName("QTPROD")]
        public string Quantidade { get; set; }
    }
}
