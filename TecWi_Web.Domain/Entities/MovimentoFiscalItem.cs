namespace TecWi_Web.Domain.Entities
{
    public class MovimentoFiscalItem
    {
        public int ID { get; set; }
        public int IDMovimentoFiscal { get; set; }
        public MovimentoFiscal MovimentoFiscal { get; set; }
        public int Sequencia { get; set; }
        public string CdProduto { get; set; }
        public decimal Qtd { get; set; }
    }
}
