namespace TecWi_Web.Domain.Entities
{
    public class PedidoItemMercoCamp
    {
        public int IdPedidoItem { get; set; }
        public int PedidoMercoCampId { get; set; }
        public int NumPedido { get; set; }
        public int SeqTransmissao { get; set; }
        public decimal Qtd { get; set; }
        public string CdProduto { get; set; }
        public decimal QtdSeparado { get; set; }
        public PedidoMercoCamp PedidoMercoCamp { get; set; }
    }
}
