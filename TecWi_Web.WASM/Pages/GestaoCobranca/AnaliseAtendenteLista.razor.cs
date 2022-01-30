using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.FrontServices;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.GestaoCobranca
{
    public partial class AnaliseAtendenteLista
    {
        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        private bool exibeSpinner = false;

        private DateTime? dtInicio { get; set; } = DateTime.Now;
        private DateTime? dtFim { get; set; } = DateTime.Now;

        private PesquisaDTO pesquisaDTO = new PesquisaDTO();

        private List<AtendenteDTO> listaAtendenteDTO = new List<AtendenteDTO>();
        private bool exibeModalContato = false;

        public List<ClienteDTO> listaClienteDTO = new List<ClienteDTO>();
        public List<ContatoCobrancaDTO> listaContatoCobrancaDTO = new List<ContatoCobrancaDTO>();
        private ContatoCobrancaDTO contatoCobrancaDTO = new ContatoCobrancaDTO();

        public AnaliseAtendenteLista()
        {
            contatoCobrancaDTO.ClienteDTO = new ClienteDTO();
        }

        private void AlteraDtInicio(Syncfusion.Blazor.Calendars.ChangedEventArgs<DateTime?> args)
        {
            if (args.Value.GetValueOrDefault() > DateTime.Now.Date)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Data inicial não pode ser maior que a data atual";
                mensagemInformativaDTO.Exibe = true;

            }
            else
            {
                dtInicio = args.Value.GetValueOrDefault();
            }
        }

        private void AlteraDtFim(Syncfusion.Blazor.Calendars.ChangedEventArgs<DateTime?> args)
        {
            if (args.Value.GetValueOrDefault() > DateTime.Now.Date)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Data final não pode ser maior que a data atual";
                mensagemInformativaDTO.Exibe = true;

            }
            else
            {
                dtFim = args.Value.GetValueOrDefault();
            }
        }

        private async Task PesquisaAtendentes()
        {
            if(dtInicio > dtFim)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "A data inicio não pode ser maior que a data final";
                mensagemInformativaDTO.Exibe = true;
                return;
            }

            pesquisaDTO.DtInicio = (DateTime)dtInicio;
            pesquisaDTO.DtFim = (DateTime)dtFim;

            exibeSpinner = true;
            ServiceResponse<List<AtendenteDTO>> serviceResponse = await atendenteFrontService.ListaPerformanceAtendentes(pesquisaDTO);
            
            exibeSpinner = false;
            
            if(!serviceResponse.Success)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
            }else
            {
                listaAtendenteDTO = serviceResponse.Data;
                foreach(var atendente in listaAtendenteDTO)
                {
                    listaClienteDTO.AddRange(atendente.clienteDTO);
                    foreach(var contato in atendente.clienteDTO)
                    {
                        listaContatoCobrancaDTO.AddRange(contato.ContatoCobrancaDTO);
                    }
                }
            }
        }

        private Query GetClientesQuery(AtendenteDTO atendenteDTO)
        {
            return new Query().Where("IdUsuario", "equal", atendenteDTO.IdUsuario);
        }

        private Query GetContatosCobranca(ClienteDTO cliente)
        {
            return  new Query().Where("Cdclifor", "equal", cliente.cdclifor).Where("IdUsuario", "equal", cliente.IdUsuario);
        }

        private void ExibeDetalhesContato(CommandClickEventArgs<ContatoCobrancaDTO> args)
        {
            contatoCobrancaDTO = args.RowData;
            contatoCobrancaDTO.DsTipoContato = contatoCobrancaDTO.TipoContato.ToString();
            exibeModalContato = true;
        }

        private void FechaModalContato()
        {
            exibeModalContato = false;
        }
    }
}
