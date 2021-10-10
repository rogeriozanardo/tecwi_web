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
        private bool exibeModalIncluiUsuario = false;
        private bool exibeModalAlteraUsuario = false;
        private bool exibeModalTrocaSenha = false;
        private bool exibeModalAplicacoes = false;
        private bool exibeModalEdicaoAplicacao = false;

        private bool? StAtivo = false;
        private bool? StAplicacaoAtiva = false;

        private string pesquisa = string.Empty;

        private UsuarioDTO usuarioDTO = new UsuarioDTO();
        private List<UsuarioAplicacaoDTO> listaUsuarioAplicacaoDTO = new List<UsuarioAplicacaoDTO>();

        private SfGrid<UsuarioAplicacaoDTO> sfGridAplicacoesUsuario = new SfGrid<UsuarioAplicacaoDTO>();

        private UsuarioAplicacaoDTO usuarioAplicacaoDTO = new UsuarioAplicacaoDTO();

        private string confirmaSenha = "";

        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        private List<UsuarioDTO> listaUsuarioDTO = new List<UsuarioDTO>();

        private int? indexPerfil { get; set; } = 0;
        private void EditaUsuario(CommandClickEventArgs<UsuarioDTO> args)
        {
            usuarioDTO = args.RowData;
            if(usuarioDTO.Login == "admin")
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "O usuário admin não pode ser alterado.";
                mensagemInformativaDTO.Exibe = true;
                return;
            }

            if (args.CommandColumn.ButtonOption.Content == "Senha")
            {
                exibeModalTrocaSenha = true;
            } else if (args.CommandColumn.ButtonOption.Content == "Aplicações")
            {

                foreach (var item in usuarioDTO.UsuarioAplicacaoDTO)
                {
                    item.DsAplicacao = item.IdAplicacao.GetDisplayName();
                }

                exibeModalAplicacoes = true;
            }else if(args.CommandColumn.ButtonOption.Content == "Editar")
            {
                StAtivo = usuarioDTO.Ativo;
                exibeModalAlteraUsuario = true;
            }
        }

        private void AlteraStAtivo(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
        {
            if(!args.Checked.Value)
            {
                usuarioDTO.Ativo = false;
            }else
            {
                usuarioDTO.Ativo = true;
            }
        }

        private void AlteraStAtivoAplicacao(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
        {
            if(!args.Checked.Value)
            {
                usuarioAplicacaoDTO.StAtivo = false;
            }
            else
            {
                usuarioAplicacaoDTO.StAtivo = true;
            }
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

            confirmaSenha = string.Empty;
            exibeModalIncluiUsuario = true;
        }

        private void FechaModalCadastro()
        {
            exibeModalIncluiUsuario = false;
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
                exibeModalTrocaSenha = false;
                exibeModalIncluiUsuario = false;
            }
            else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
            }

            mensagemInformativaDTO.Exibe = true;
        }

        private void EditaAplicacoesUsuario(CommandClickEventArgs<UsuarioAplicacaoDTO> args)
        {
            usuarioAplicacaoDTO = args.RowData;
            StAplicacaoAtiva = usuarioAplicacaoDTO.StAtivo;
            exibeModalEdicaoAplicacao = true;
        }

        private void FecharTrocaSenha()
        {
            exibeModalTrocaSenha = false;
        }

        private void FechaModalAlteraUsuario()
        {
            exibeModalAlteraUsuario = false;
        }

        private async Task SalvarDadosUsuario()
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            exibeSpinner = true;
            serviceResponse = await usuarioFrontService.UpdateJustInfoAsync(usuarioDTO);

            
            if(serviceResponse.Success)
            {
                mensagemInformativaDTO.Titulo = "Sucesso";
                mensagemInformativaDTO.Mensagem = "Cadastro salvo com sucesso";
                await PesquisaUsuarios();
                exibeSpinner = false;
                exibeModalAlteraUsuario = false;
                mensagemInformativaDTO.Exibe = true;
            }else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                exibeSpinner = false;
                mensagemInformativaDTO.Exibe = true;
            }

        }

        private void FecharAlteraApicacoes()
        {
            exibeModalAplicacoes = false;
        }

        private async Task SalvarAplicacoes()
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            exibeSpinner = true;

            serviceResponse = await usuarioFrontService.AtualizaAplicacoesUsuario(usuarioDTO.UsuarioAplicacaoDTO);

            exibeSpinner = false;
            if(serviceResponse.Success)
            {
                mensagemInformativaDTO.Titulo = "Sucesso";
                mensagemInformativaDTO.Mensagem = "Aplicações do usuário atualizadas com sucesso.";
                await PesquisaUsuarios();
                exibeModalAplicacoes = false;
                mensagemInformativaDTO.Exibe = true;
            }else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
            }
        }

        private void FechaAlteracaoAplicacao()
        {
            exibeModalEdicaoAplicacao = false;
        }

        private void AtualizaListaAplicao()
        {
            int index = usuarioDTO.UsuarioAplicacaoDTO.FindIndex(x => x.IdAplicacao == usuarioAplicacaoDTO.IdAplicacao);
            if(index >= 0)
            {
                usuarioDTO.UsuarioAplicacaoDTO[index].StAtivo = (bool)StAplicacaoAtiva;
                usuarioDTO.UsuarioAplicacaoDTO[index].IdPerfil = usuarioAplicacaoDTO.IdPerfil;
            }
            sfGridAplicacoesUsuario.Refresh();
            exibeModalEdicaoAplicacao = false;
        }

    }
}
