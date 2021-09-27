using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Shared.Utility
{
    public class Utilitarios
    {
        public static bool ChecaSeStringTemSomenteNumeros(string texto)
        {
            if (texto.Where(c => char.IsNumber(c)).Count() > 0)
                return true;
            else
                return false;
        }
    }
}
