using System;
using System.Collections.Generic;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Domain.Entities
{
    public class PedidoMercoCamp
    {
        public PedidoMercoCamp()
        {
            PedidoItens = new List<PedidoItemMercoCamp>();
        }
        public int IdPedidoMercoCamp { get; set; }
        public DateTime DataEnvio { get; set; }
        public int NumPedido { get; set; }
        public StatusPedidoMercoCamp Status { get; set; }
        public int Volume { get; set; }
        public decimal Peso { get; set; }
        public int SeqTransmissao { get; set; }
        public string CdFilial { get; set; }
        public List<PedidoItemMercoCamp> PedidoItens { get; set; }
    }
}
