using System;

namespace TecWi_Web.Shared.Filters
{
    public class ClientePagarReceberFilter : FilterBase
    {
        public Guid IdUsuario { get; set; }
        public int? cdclifor { get; set; }
        public string inscrifed { get;  set; }
        public string fantasia { get;  set; }
        public string razao { get;  set; }
        public string numlancto { get;  set; }
        public string cdfilial { get;  set; }
        public DateTime? dtemissaoStart { get;  set; } 
        public DateTime? dtemissaoEnd { get;  set; }
        public DateTime? dtvenctoStart { get;  set; }
        public DateTime? dtvenctoEnd { get;  set; }
        public string NotasFiscais { get;  set; }
    }
}
