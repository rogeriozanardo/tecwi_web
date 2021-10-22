using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Domain.Enums;
using TecWi_Web.FrontServices;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.GestaoCobranca
{
    public partial class ContatosLista
    {
        bool exibeSpinner = false;
        bool? agendaContatoFuturo = false;
        bool habilitaAgenda = false;
        DateTime DataAgenda = DateTime.Now;
        
        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        private int? indexTipoContato { get; set; } = 0;

        private ContatoCobrancaDTO contatoCobrancaDTO = new ContatoCobrancaDTO();


        private SfGrid<ClienteContatoDTO> sfGridPessoasContato = new SfGrid<ClienteContatoDTO>();
        private SfTab sfTabContato = new SfTab();

        private ClienteContatoDTO clienteContatoDTO = new ClienteContatoDTO();
        private bool exibeModalPessoaContato = false;



        private class TipoContato
        {
            public int Id { get; set; }
            public string DsTipoContato { get; set; }
        }

        private List<TipoContato> tipoContato = new List<TipoContato>();

        private bool exibeModalContato = false;

        private ClienteDTO clienteDTO = new ClienteDTO();

        private async Task InciaContatos()
        {
            var perfil = Config.usuarioDTO.UsuarioAplicacaoDTO.Where(x => x.IdAplicacao == Domain.Enums.IdAplicacao.Cobranca).FirstOrDefault(); ;

            if (perfil.IdPerfil != Domain.Enums.IdPerfil.Operador)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Seu usuário não tem perfil para realizar cobranças.";
                mensagemInformativaDTO.Exibe = true;
                return;
            }

            ServiceResponse<ClienteDTO> serviceResponse = new ServiceResponse<ClienteDTO>();

            exibeSpinner = true;

            serviceResponse = await clienteFrontService.BuscaProximoClienteFilaPorUsuario();

            exibeSpinner = false;

            if (serviceResponse.Data == null)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Não há clientes vinculados ao seu usuário";
                mensagemInformativaDTO.Exibe = true;
                return;
            }



            if (serviceResponse.Success)
            {
                clienteDTO = serviceResponse.Data;
                contatoCobrancaDTO = new ContatoCobrancaDTO();
                exibeModalContato = true;
            }
            else
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
            if(indexTipoContato < 0)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Informe o tipo de contato";
                mensagemInformativaDTO.Exibe = true;
                return;
            }

            exibeSpinner = true;

            contatoCobrancaDTO.Cdclifor = clienteDTO.cdclifor;
            contatoCobrancaDTO.IdUsuario = Config.usuarioDTO.IdUsuario;
            contatoCobrancaDTO.DtContato = DateTime.Now;
            contatoCobrancaDTO.UsuarioDTO = Config.usuarioDTO;
            contatoCobrancaDTO.ClienteDTO = clienteDTO;
            contatoCobrancaDTO.DtAgenda = DataAgenda;
            
            foreach(var item in clienteDTO.PagarReceberDTO)
            {
                contatoCobrancaDTO.ContatoCobrancaLancamentoDTO.Add(new ContatoCobrancaLancamentoDTO()
                {
                    Numlancto = item.numlancto,
                    Sq = item.sq,
                    CdFilial = item.cdfilial
                });
            }

            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            serviceResponse = await cobrancaFrontService.GravaContato(contatoCobrancaDTO);
            if(!serviceResponse.Success)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
                exibeSpinner = false;
                return;
            }
            else
            {
                contatoCobrancaDTO = new ContatoCobrancaDTO();
                DataAgenda = DateTime.Now;
                agendaContatoFuturo = false;
                indexTipoContato = -1;
                habilitaAgenda = false;
                clienteDTO = new ClienteDTO();
                clienteDTO.PagarReceberDTO = new List<PagarReceberDTO>();

                ServiceResponse<ClienteDTO> serviceResponseProxContato = new ServiceResponse<ClienteDTO>();

                serviceResponseProxContato = await clienteFrontService.BuscaProximoClienteFilaPorUsuario();

                exibeSpinner = false;

                if (serviceResponseProxContato.Data == null)
                {
                    exibeModalContato = false;
                    mensagemInformativaDTO.Titulo = "Atenção";
                    mensagemInformativaDTO.Mensagem = "Não há clientes na sua fila de contados";
                    mensagemInformativaDTO.Exibe = true;
                    
                    return;
                }

                if(serviceResponseProxContato.Success)
                {
                    clienteDTO = serviceResponseProxContato.Data;
                    contatoCobrancaDTO = new ContatoCobrancaDTO();
                }else
                {
                    mensagemInformativaDTO.Titulo = "Atenção";
                    mensagemInformativaDTO.Mensagem = serviceResponseProxContato.Message;
                    mensagemInformativaDTO.Exibe = true;
                }
            }
        }

        private void SelecaoTipoContato(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, TipoContato> args)
        {
            if (args.Value != null)
            {
                contatoCobrancaDTO.TipoContato = (TipoContatoEnum)args.ItemData.Id;
            }
        }
        protected override async Task OnInitializedAsync()
        {

            tipoContato = ((TipoContatoEnum[])Enum.GetValues(typeof(TipoContatoEnum))).Select(c => new TipoContato()
            {
                Id = (int)c,
                DsTipoContato = c.ToString()
            }).ToList();

            indexTipoContato = -1;
        }

        private void AlteraDtAgenda(Syncfusion.Blazor.Calendars.ChangedEventArgs<DateTime?> args)
        {
            if(args.Value.GetValueOrDefault() < DateTime.Now.Date)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Data da agenda não pode ser menor que a data atual";
                mensagemInformativaDTO.Exibe = true;
                
            }else
            {
                DataAgenda = args.Value.GetValueOrDefault();
                contatoCobrancaDTO.DtAgenda = DataAgenda;
            }
           
        }

        private void AlteraStatusAgenda(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
        {
            if(!args.Checked.Value)
            {
                DataAgenda = DateTime.Now;
                habilitaAgenda = false;
            }else
            {
                habilitaAgenda = true;
            }
        }

        private void IncluirPessoaContato()
        {
            clienteContatoDTO = new ClienteContatoDTO();
            clienteContatoDTO.Cdclifor = clienteDTO.cdclifor;
            exibeModalPessoaContato = true;
        }

        private async Task EditaExcluiPessoaContato(CommandClickEventArgs<ClienteContatoDTO> args)
        {
            clienteContatoDTO = args.RowData;

            if (args.CommandColumn.ButtonOption.Content == "Editar")
            {
                exibeModalPessoaContato = true;
            }

        }


        private async Task SalvarPessoaContato()
        {
            if (string.IsNullOrEmpty(clienteContatoDTO.Nome))
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Campo nome é obrigatório";
                mensagemInformativaDTO.Exibe = true;
                return;
            }
            ServiceResponse<Guid> serviceResponse = new ServiceResponse<Guid>();
            exibeSpinner = true;

            serviceResponse = await clienteFrontService.SalvaContatoCliente(clienteContatoDTO);

            exibeSpinner = false;
            if (serviceResponse.Success)
            {
                int index = clienteDTO.ClienteContatoDTO.FindIndex(x => x.IdClienteContato == serviceResponse.Data);
                if (index >= 0)
                {
                    clienteDTO.ClienteContatoDTO.RemoveAt(index);
                    clienteDTO.ClienteContatoDTO.Add(clienteContatoDTO);
                    sfGridPessoasContato.Refresh();
                }
                else
                {
                    clienteContatoDTO.IdClienteContato = serviceResponse.Data;
                    clienteDTO.ClienteContatoDTO.Add(clienteContatoDTO);
                    sfGridPessoasContato.Refresh();
                }
                await sfTabContato.GetTabItem(0);
                exibeModalPessoaContato = false;

                mensagemInformativaDTO.Titulo = "Sucesso.";
                mensagemInformativaDTO.Mensagem = "Pessoa de contato salvo com sucesso.";
                mensagemInformativaDTO.Exibe = true;
            }
            else
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
            }

        }
        private void FecharModalPessoaContato()
        {
            exibeModalPessoaContato = false;
        }
    }
}
