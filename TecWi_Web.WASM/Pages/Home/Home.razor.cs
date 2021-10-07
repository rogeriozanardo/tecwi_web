using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.FrontServices;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.Home
{
    partial class Home
    {
        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();

        private bool exibeModalTrocaSenha = false;
        public void GestaoCobranca_OnClick()
        {
            int index = Config.usuarioDTO.UsuarioAplicacaoDTO.FindIndex(x => x.IdAplicacao == Domain.Enums.IdAplicacao.Cobranca);

            if (index >= 0)
            {
                Navigation.NavigateTo("/GestaoCobranca");
            }
            else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Você não tem acesso a esta aplicação.";
                mensagemInformativaDTO.Exibe = true;
                return;
            }
        }

        public void GestaoLogistica_OnClick()
        {
            int index = Config.usuarioDTO.UsuarioAplicacaoDTO.FindIndex(x => x.IdAplicacao == Domain.Enums.IdAplicacao.Logistica);

            if (index >= 0)
            {
                Navigation.NavigateTo("/GestaoLogistica");
            }
            else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Você não tem acesso a esta aplicação.";
                mensagemInformativaDTO.Exibe = true;
                return;
            }
        }

        public void Configuracoes_OnClick()
        {

            int index = Config.usuarioDTO.UsuarioAplicacaoDTO.FindIndex(x => x.IdAplicacao == Domain.Enums.IdAplicacao.Configuracoes && x.StAtivo == true && x.IdPerfil == Domain.Enums.IdPerfil.Gestor);

            if (index >= 0)
            {
                Navigation.NavigateTo("/Configuracoes");
            }
            else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Você não tem acesso a esta aplicação.";
                mensagemInformativaDTO.Exibe = true;
                return;
            }
        }

        private void ExibeModalTrocaSenha()
        {
            if(Config.usuarioDTO.Login=="admin")
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Opção não disponível para o usuário admim";
                mensagemInformativaDTO.Exibe = true;
            }else
            {
                exibeModalTrocaSenha = true;
            }
        }

        private void FecharTrocaSenha()
        {
            exibeModalTrocaSenha = false;
        }
    }
}
