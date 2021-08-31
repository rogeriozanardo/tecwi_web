using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Domain.Entities
{
    public class Cliente
    {
        public int cdclifor { get; private set; }
        public string inscrifed { get; private set; }
        public string fantasia { get; private set; }
        public string razao { get; private set; }
        public int ddd { get; private set; }
        public string fone1 { get; private set; }
        public string fone2 { get; private set; }
        public string email { get; private set; }
        public string cidade { get; private set; }

        public Cliente()
        {

        }
    }
}
