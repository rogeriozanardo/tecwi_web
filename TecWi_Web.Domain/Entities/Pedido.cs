using System;
using System.Collections.Generic;

namespace TecWi_Web.Domain.Entities
{
    public class Pedido
    {
        public int ID { get; set; }
        public string cdempresa { get; set; }
        public string cdfilial { get; set; }
        public string cdcliente { get; set; }
        public int nummovimento { get; set; }
        public string nummovcliente { get; set; }
        public DateTime? dtinicio { get; set; }
        public string stpendencia { get; set; }
        public string cdvendedor { get; set; }
        public string stpagafrete { get; set; }
        public string cdtransportadora { get; set; }
        public string cdtransportadoraredespacho { get; set; }
        public string stpagafreteredespacho { get; set; }
        public List<PedidoItem> PedidoItem { get; set; }
    }
}
