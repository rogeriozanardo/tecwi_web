using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.Configuracoes
{
    public partial class UsuarioLista
    {
        bool exibeSpinner = false;
        bool exibeModalIncluiAlteraUsuario = false;

        private UsuarioDTO usuarioDTO = new UsuarioDTO();
        private string confirmaSenha = "";

        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        private List<UsuarioDTO> listaUsuarioDTO = new List<UsuarioDTO>();

        
        private void ModalEditaUsuario(CommandClickEventArgs<UsuarioDTO> args)
        {

        }

        protected override async Task OnInitializedAsync()
        {


        }

        private async Task PesquisaUsuarios()
        {

        }

        private void ModalIncluirUsuario()
        {
            usuarioDTO = new UsuarioDTO();
            confirmaSenha = string.Empty;
            exibeModalIncluiAlteraUsuario = true;
        }

        private void FechaModalCadastro()
        {
            exibeModalIncluiAlteraUsuario = false;
        }

        private async Task SalvarUsuario()
        {
            if (string.IsNullOrEmpty(usuarioDTO.Senha))
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Informe a senha";
                mensagemInformativaDTO.Exibe = true;
                return;
            }

            if (string.IsNullOrEmpty(confirmaSenha))
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Informe a confirmação de senha";
                mensagemInformativaDTO.Exibe = true;
                return;
            }

            if (usuarioDTO.Senha != confirmaSenha)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "As senhas não conferem";
                mensagemInformativaDTO.Exibe = true;
                return;
            }

            exibeSpinner = true;
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            serviceResponse = await usuarioFrontService.SalvarUsuario(usuarioDTO);

            exibeSpinner = false;
            if (serviceResponse.Success)
            {
                mensagemInformativaDTO.Titulo = "Sucesso";
                mensagemInformativaDTO.Mensagem = "Cadastro salvo";
                exibeModalIncluiAlteraUsuario = false;
            }
            else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
            }

            mensagemInformativaDTO.Exibe = true;
        }
    }
}
