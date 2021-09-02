using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Domain.Entities
{
    public class PagarReceber
    {
        public PagarReceber()
        {

        }

        public Cliente cliente { get; set; }

        public int SeqID { get; private set; }
        public string numlancto { get; private set; }
        public int sq { get; set; }
        public string cdfilial { get; private set; }
        public DateTime dtemissao { get; private set; }
        public DateTime dtvencto { get; private set; }
        public decimal valorr { get; private set; }
        public string NotasFiscais { get; private set; }
        public int cdclifor { get; private set; }
        public string inscrifed { get; private set; }
        public string fantasia { get; private set; }
        public string razao { get; private set; }
        public string ddd { get; private set; }
        public string fone1 { get; private set; }
        public string fone2 { get; private set; }
        public string email { get; private set; }
        public string cidade { get; private set; }


    }
}
