using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class ConfirmacaoPedidoDTO
    {
        public ConfirmacaoPedidoDTO()
        {
            Itens = new List<ConfirmacaoPedidoItemDTO>();
        }

        [JsonIgnore()]
        public int ID { get; set; }
        public string NumeroPedidoCliente { get; set; }
        public string CNPJEmitente { get; set; }
        public DateTime Sincronizacao { get; set; }

        [JsonPropertyName("ITENS")]
        public List<ConfirmacaoPedidoItemDTO> Itens { get; set; }
    }
}
