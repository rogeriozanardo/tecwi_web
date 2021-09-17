using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Shared.Enums
{
    public enum AplicacaoEnum
    {
        [Display(Name = "Configurações")]
        Configuracoes = 1,

        [Display(Name = "Gestão de Cobrança")]
        GestaoCobranca = 2,

        [Display(Name = "Gestão de Logística")]
        GestaoLogistica = 3,


    }
}
