﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TecWi_Web.WASM.Pages.Configuracoes
{
    public partial class ConfiguracoesLayout
    {
        public void Sair_OnClick()
        {
            Navigation.NavigateTo("/");
        }

        public void TrocarModulo_OnClick()
        {
            Navigation.NavigateTo("/Home");
        }
    }
}
