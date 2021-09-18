using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.Componentes
{
    public partial class MensagemInformativa
    {
        [Parameter]
        public MensagemInformativaDTO mensagemInformativaDTO { get; set; }

        void FechaMensagem()
        {
            mensagemInformativaDTO.Exibe = false;
        }
    }
}
