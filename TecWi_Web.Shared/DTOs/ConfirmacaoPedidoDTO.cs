using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class ConfirmacaoPedidoDTO
    {
        public ConfirmacaoPedidoDTO()
        {
           
        }

        public string NumeroPedidoCliente { get; set; }
        public string CNPJEmitente { get; set; }
        public DateTime Sincronizacao { get; set; }

        [JsonPropertyName("ITENS")]
        public List<PedidoItemMercoCampDTO> Itens { get; set; }
    }
}
