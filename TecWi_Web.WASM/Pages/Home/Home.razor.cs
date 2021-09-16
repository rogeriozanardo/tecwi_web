using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TecWi_Web.WASM.Pages.Home
{
    partial class Home
    {
        public void GestaoCobranca_OnClick()
        {
            Navigation.NavigateTo("/GestaoCobranca");
        }

        public void GestaoLogistica_OnClick()
        {
            Navigation.NavigateTo("/GestaoLogistica");
        }

        public void Configuracoes_OnClick()
        {
            Navigation.NavigateTo("/Configuracoes");
        }
    }
}
