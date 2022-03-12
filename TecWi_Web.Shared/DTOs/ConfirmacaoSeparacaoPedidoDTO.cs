using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class ConfirmacaoSeparacaoPedidoDTO
    {
        [JsonPropertyName("CORPEM_WMS_CONF_SEP")]
        public CORPEMWMSCONFSEPDTO CORPEMWMSCONFSEP { get; set; }
    }
}
