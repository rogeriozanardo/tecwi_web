using System;

namespace TecWi_Web.Shared.DTOs
{
    public class PagarReceberDTO
    {
        public int SeqID { get; set; }
        public string numlancto { get; set; }
        public int sq { get; set; }
        public string cdfilial { get; set; }
        public DateTime dtemissao { get; set; }
        public DateTime dtvencto { get; set; }
        public decimal valorr { get; set; }
        public string NotasFiscais { get; set; }
        public int cdclifor { get; set; }
        public string inscrifed { get; set; }
        public string fantasia { get; set; }
        public string razao { get; set; }
        public string ddd { get; set; }
        public string fone1 { get; set; }
        public string fone2 { get; set; }
        public string email { get; set; }
        public string cidade { get; set; }
    }
}
