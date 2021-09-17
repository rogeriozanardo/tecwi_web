using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Shared.Enums
{
    public enum PerfilEnum
    {
        [Display(Name = "Gestor")]
        Gestor = 1,

        [Display(Name = "Operador")]
        Operador = 2
    }
}
