using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class CORPEM_WMS_CONF_SEP
    {
        public CORPEM_WMS_CONF_SEP()
        {
            Itens = new List<ConfirmacaoPedidoItemDTO>();
        }

        [JsonPropertyName("CGCEMINF")]
        public string CGCEMINF { get; set; }

        [JsonPropertyName("CGCCLIWMS")]
        public string CGCCLIWMS { get; set; }

        [JsonPropertyName("NUMPEDCLI")]
        public string NUMPEDCLI { get; set; }

        [JsonPropertyName("ESPECIE")]
        public string ESPECIE { get; set; }

        [JsonPropertyName("PESOVOL")]
        public string PESOVOL { get; set; }

        [JsonPropertyName("M3VOL")]
        public string M3VOL { get; set; }

        [JsonPropertyName("QTVOL")]
        public string QTVOL { get; set; }

        [JsonPropertyName("CGCTRANSP")]
        public string CGCTRANSP { get; set; }

        [JsonPropertyName("DTFIMCHECK")]
        public string DTFIMCHECK { get; set; }

        [JsonPropertyName("URLRAST")]
        public string URLRAST { get; set; }

        [JsonPropertyName("ITENS")]
        public List<ConfirmacaoPedidoItemDTO> Itens { get; set; }
    }
}
