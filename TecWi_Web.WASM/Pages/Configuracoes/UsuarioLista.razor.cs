using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.WASM.Pages.Configuracoes
{
    public partial class UsuarioLista
    {
        private bool exibeSpinner = false;
        private bool exibeModalIncluiAlteraUsuario = false;
        private bool habilitaCampoLogin = false;
        private string pesquisa = string.Empty;

        private UsuarioDTO usuarioDTO = new UsuarioDTO();
        private string confirmaSenha = "";

        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        private List<UsuarioDTO> listaUsuarioDTO = new List<UsuarioDTO>();

        
        private void ModalEditaUsuario(CommandClickEventArgs<UsuarioDTO> args)
        {
            usuarioDTO = args.RowData;
            if(usuarioDTO.Login == "admin")
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "O usuário admin não pode ser alterado.";
                mensagemInformativaDTO.Exibe = true;
                return;
            }

            foreach (var item in usuarioDTO.UsuarioAplicacaoDTO)
            {
                item.DsAplicacao = item.IdAplicacao.GetDisplayName();
            }

            confirmaSenha = string.Empty;

            exibeModalIncluiAlteraUsuario = true;
        }

        protected override async Task OnInitializedAsync()
        {


        }

        private async Task PesquisaUsuarios()
        {
            ServiceResponse<List<UsuarioDTO>> serviceResponse = new ServiceResponse<List<UsuarioDTO>>();
            exibeSpinner = true;
            UsuarioFilter usurioFilter = new UsuarioFilter();
            usurioFilter.Nome = pesquisa;
            serviceResponse = await usuarioFrontService.GetAllAsync(usurioFilter);

            exibeSpinner = false;
            if(serviceResponse.Success)
            {
                listaUsuarioDTO = serviceResponse.Data;
            }else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
            }


        }

        private void ModalIncluirUsuario()
        {
            usuarioDTO = new UsuarioDTO();
            usuarioDTO.UsuarioAplicacaoDTO = new List<UsuarioAplicacaoDTO>();

            usuarioDTO.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Cobranca, IdPerfil = IdPerfil.Operador, StAtivo = false });
            usuarioDTO.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Comercial, IdPerfil = IdPerfil.Operador, StAtivo = false });
            usuarioDTO.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Configuracoes, IdPerfil = IdPerfil.Operador, StAtivo = false });
            usuarioDTO.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Financeiro, IdPerfil = IdPerfil.Operador, StAtivo = false });
            usuarioDTO.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Logistica, IdPerfil = IdPerfil.Operador, StAtivo = false });

            foreach(var item in usuarioDTO.UsuarioAplicacaoDTO)
            {
                item.DsAplicacao = item.IdAplicacao.GetDisplayName();
            }

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

        private void EditaAplicacoesUsuario()
        {

        }
    }
}
