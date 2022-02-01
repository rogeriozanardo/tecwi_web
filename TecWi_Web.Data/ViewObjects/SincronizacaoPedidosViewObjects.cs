using System.Collections.Generic;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.ValueObjects
{
    public class SincronizacaoPedidosViewObjects
    {
        public SincronizacaoPedidosViewObjects()
        {
            Pedidos = new List<Pedido>();
            Transportadoras = new List<Transportadora>();
            Empresas = new List<Empresa>();
            Vendedores = new List<Vendedor>();
        }
        public IEnumerable<Pedido> Pedidos { get; set; }
        public IEnumerable<Transportadora> Transportadoras { get; set; }
        public IEnumerable<Empresa> Empresas { get; set; }
        public IEnumerable<Vendedor> Vendedores { get; set; }
    }
}
