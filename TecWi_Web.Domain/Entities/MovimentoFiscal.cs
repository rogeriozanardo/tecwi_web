using System;
using System.Collections.Generic;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Domain.Entities
{
    public class MovimentoFiscal
    {
        public int ID { get; set; }
        public int NumMovimento { get; set; }
        public string CdFilial { get; set; }
        public int NumeroNota { get; set; }
        public string Serie { get; set; }
        public DateTime DataEmissao { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal QtdVolume { get; set; }
        public string ChaveAcesso { get; set; }
        public StatusTransmissaoMovimentoFiscal Status { get; set; }
        public List<MovimentoFiscalItem> ItensMovimentoFiscal { get; set; }
    }
}
