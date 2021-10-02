using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.Componentes
{
    public partial class TrocaSenhaUsuario
    {
        [Parameter]
        public bool exibe { get; set; }

        private bool exibeSpinner = false;
        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();

        private void Fechar()
        {
            exibe = false;
        }

    }
}
