using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.FrontServices;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.GestaoCobranca
{
    public partial class ClienteLista
    {
        private List<ClienteDTO> listaClienteDTO = new List<ClienteDTO>();
        private ClienteDTO clienteDTO = new ClienteDTO();
        private bool perfilGestor = false;

        private bool exibeModalCliente = false;

        private AnotacaoDTO anotacaoDTO = new AnotacaoDTO();

        private SfTab tabCliente = new SfTab();

        private string pesquisa = string.Empty;
        bool exibeSpinner = false;
        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        private async Task PesquisaCliente()
        {
            if (string.IsNullOrEmpty(pesquisa))
            {
                return;
            }

            ServiceResponse<List<ClienteDTO>> serviceResponse = new ServiceResponse<List<ClienteDTO>>();

            exibeSpinner = true;

            serviceResponse = await clienteFrontService.PesquisaCliente(pesquisa);

            exibeSpinner = false;

            if (serviceResponse.Success)
            {
                listaClienteDTO = serviceResponse.Data;

            }
            else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
            }
        }

        private void ExibeTelaCadastroCliente(CommandClickEventArgs<ClienteDTO> args)
        {
            clienteDTO = args.RowData;
            tabCliente.Select(0);

            exibeModalCliente = true;
        }

        private void FechaModalCliente()
        {
            exibeModalCliente = false;
        }

        private async Task SalvarCliente()
        {

        }

        private void ExibeAnotacaoContato(CommandClickEventArgs<ContatoCobrancaDTO> args)
        {
            anotacaoDTO.DtContato = args.RowData.DtContato;
            anotacaoDTO.Anotacao = args.RowData.Anotacao;
            anotacaoDTO.exibe = true;
        }

        protected override async Task OnInitializedAsync()
        {
            var perfil = Config.usuarioDTO.UsuarioAplicacaoDTO.Where(x => x.IdAplicacao == Domain.Enums.IdAplicacao.Cobranca).FirstOrDefault(); ;

            if (perfil.IdPerfil == Domain.Enums.IdPerfil.Gestor)
            {
                perfilGestor = true;
            } else
            {
                perfilGestor = false;
            }

        }
    }
}
