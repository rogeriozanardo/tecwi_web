using System;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Shared.Filters
{
    public class ContatoCobrancaLancamentoFilter : FilterBase
    {
        public int IdContato { get; set; }
        public string Numlancto { get; set; }
        public int Sq { get; set; }
        public string Cdfilial { get; set; }
    }
}
