using System.Collections.Generic;

namespace TecWi_Web.Shared.DTOs
{
    public class MovimentoFiscalDTO
    {
        public MovimentoFiscalDTO()
        {
            ItensMovimentoFiscal = new List<MovimentoFiscalItemDTO>();
        }
        public int ID { get; set; }
        public string CNPJEmitente { get; set; }
        public string NumPedidoCliente  { get; set; }
        public string NumNotaFiscal { get; set; }
        public string SerieNotaFiscal { get; set; }
        public string DataEmissao { get; set; }
        public string ValorTotalNota { get; set; }
        public string QtdVolume { get; set; }
        public string ChaveNF { get; set; }
        public List<MovimentoFiscalItemDTO> ItensMovimentoFiscal { get; set; }
    }
}
