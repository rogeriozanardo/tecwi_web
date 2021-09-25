using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.GestaoCobranca
{
    public partial class ContatosLista
    {
        bool exibeSpinner = false;
        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();

        private bool exibeModalContato = false;

        private ClienteDTO clienteDTO = new ClienteDTO();

        private async Task InciaContatos()
        {
            ServiceResponse<ClienteDTO> serviceResponse = new ServiceResponse<ClienteDTO>();

            exibeSpinner = true;
            
            serviceResponse = await clienteFrontService.BuscaProximoClienteFilaPorUsuario();

            exibeSpinner = false;

            if (serviceResponse.Success)
            {
                clienteDTO = serviceResponse.Data;
                exibeModalContato = true;
            }else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
            }
        }

        private void FechaModalContato()
        {
            exibeModalContato = false;
        }

        private async Task SalvareBuscaProximoCliente()
        {

        }
    }
}
