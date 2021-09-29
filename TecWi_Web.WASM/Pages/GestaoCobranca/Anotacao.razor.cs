using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.GestaoCobranca
{
    public partial class Anotacao
    {
        [Parameter]
        public AnotacaoDTO anotacaoDTO { get; set; }
  
        void FechaAnotacao()
        {
            anotacaoDTO.exibe = false;
        }
    }
}
