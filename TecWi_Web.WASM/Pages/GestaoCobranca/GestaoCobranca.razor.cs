using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.GestaoCobranca
{
    public partial class GestaoCobranca
    {
        bool exibeSpinner = false;
        
        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();

        private DateTime DtUltAtualicacaoCobranca;
        private async Task AtualizarDadosCobranca()
        {
            ServiceResponse<DateTime> serviceResponse = new ServiceResponse<DateTime>();
            exibeSpinner = true;
            
            serviceResponse = await cobrancaFrontService.PopulateData();
            DtUltAtualicacaoCobranca = serviceResponse.Data;

            exibeSpinner = false;

            if(serviceResponse.Success)
            {
                mensagemInformativaDTO.Titulo = "Sucesso!";
                mensagemInformativaDTO.Mensagem = "Dados de cobrança atualizados.";
            }else
            {
                mensagemInformativaDTO.Titulo = "Atenção!";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
            }

            mensagemInformativaDTO.Exibe = true;

        }
    }
}
