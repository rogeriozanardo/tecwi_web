using System;

namespace TecWi_Web.Shared.Filters
{
    public class ClientePagarReceberFilter : FilterBase
    {
        public Guid IdUsuario { get; set; }
        public int? cdclifor { get; set; }
        public string inscrifed { get; private set; }
        public string fantasia { get; private set; }
        public string razao { get; private set; }
        public string numlancto { get; private set; }
        public string cdfilial { get; private set; }
        public DateTime? dtemissaoStart { get; private set; } 
        public DateTime? dtemissaoEnd { get; private set; }
        public DateTime? dtvenctoStart { get; private set; }
        public DateTime? dtvenctoEnd { get; private set; }
        public string NotasFiscais { get; private set; }
    }
}
