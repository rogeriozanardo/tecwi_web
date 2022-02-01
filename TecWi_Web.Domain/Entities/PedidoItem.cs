namespace TecWi_Web.Domain.Entities
{
    public class PedidoItem
    {
        public int ID { get; set; }
        public int IDPedido { get; set; }
        public string cdempresa { get; set; }
        public string cdfilial { get; set; }
        public int nummovimento { get; set; }
        public int seq { get; set; }
        public string cdproduto { get; set; }
        public string tpregistro { get; set; }
        public decimal? qtdsolicitada { get; set; }
        public decimal? qtdprocessada { get; set; }
        public decimal? VlVenda { get; set; }
        public decimal? vlcalculado { get; set; }
        public string uporigem { get; set; }
        public Pedido Pedido { get; set; }
    }
}
