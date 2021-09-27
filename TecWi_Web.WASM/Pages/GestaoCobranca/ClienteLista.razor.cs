using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.GestaoCobranca
{
    public partial class ClienteLista
    {
        private List<ClienteDTO> listaClienteDTO = new List<ClienteDTO>();
        private string pesquisa = string.Empty;
        bool exibeSpinner = false;
        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        private async Task PesquisaCliente()
        {
            if(string.IsNullOrEmpty(pesquisa))
            {
                return;
            }

            ServiceResponse<List<ClienteDTO>> serviceResponse = new ServiceResponse<List<ClienteDTO>>();

            exibeSpinner = true;

            serviceResponse = await clienteFrontService.PesquisaCliente(pesquisa);

            exibeSpinner = false;

            if(serviceResponse.Success)
            {
                listaClienteDTO = serviceResponse.Data;
            }else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
            }
        }

        private void ExibeTelaCadastroCliente()
        {

        }
    }
}
