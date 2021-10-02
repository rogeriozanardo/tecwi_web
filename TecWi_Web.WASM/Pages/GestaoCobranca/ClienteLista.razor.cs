using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Domain.Enums;
using TecWi_Web.FrontServices;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.WASM.Pages.GestaoCobranca
{
    public partial class ClienteLista
    {
        private List<ClienteDTO> listaClienteDTO = new List<ClienteDTO>();
        private ClienteDTO clienteDTO = new ClienteDTO();
        private bool perfilGestor = false;
        private List<UsuarioDTO> listaAtendentes = new List<UsuarioDTO>();

        bool habilitaAgenda = false;

        DateTime DataAgenda = DateTime.Now;

        private bool exibeModalCliente = false;

        private AnotacaoDTO anotacaoDTO = new AnotacaoDTO();

        private SfTab tabCliente = new SfTab();

        private string pesquisa = string.Empty;
        private bool exibeSpinner = false;
        private bool exibeModalContato = false;

        private MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        private ContatoCobrancaDTO contatoCobrancaDTO = new ContatoCobrancaDTO();
        private int? indexTipoContato { get; set; } = 0;
        private int? indexAtendente { get; set; } = 0;
        bool? agendaContatoFuturo = false;
        private class TipoContato
        {
            public int Id { get; set; }
            public string DsTipoContato { get; set; }
        }

        private List<TipoContato> tipoContato = new List<TipoContato>();

        private async Task PesquisaCliente()
        {
            if (string.IsNullOrEmpty(pesquisa))
            {
                return;
            }

            ServiceResponse<List<ClienteDTO>> serviceResponse = new ServiceResponse<List<ClienteDTO>>();

            exibeSpinner = true;

            serviceResponse = await clienteFrontService.PesquisaCliente(pesquisa);
            if (listaAtendentes.Count == 0)
            {
                ServiceResponse<List<UsuarioDTO>> serviceResponseAtendentes = new ServiceResponse<List<UsuarioDTO>>();

                UsuarioFilter usuarioFilter = new UsuarioFilter();
                usuarioFilter.IdAplicacao = IdAplicacao.Cobranca;
                usuarioFilter.IdPerfil = IdPerfil.Operador;

                serviceResponseAtendentes = await usuarioFrontService.GetAllAsync(usuarioFilter);

                if (serviceResponseAtendentes.Success)
                {
                    listaAtendentes = serviceResponseAtendentes.Data;
                }
            }

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
            foreach(var item in clienteDTO.ContatoCobrancaDTO)
            {
                item.NomeAtendente = item.UsuarioDTO.Nome;
            }

            int indexUsuario = listaAtendentes.FindIndex(x => x.IdUsuario == clienteDTO.UsuarioDTO.IdUsuario);
            if(indexUsuario >= 0)
            {
                indexAtendente = indexUsuario;
            }

            tabCliente.Select(0);

            exibeModalCliente = true;
        }

        private void FechaModalCliente()
        {
            indexAtendente = -1;
            clienteDTO = new ClienteDTO();
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

        private void RegistrarContato()
        {
            exibeModalContato = true;
            indexTipoContato = -1;
            DataAgenda = DateTime.Now;
            contatoCobrancaDTO = new ContatoCobrancaDTO();
        }

        private void SelecaoTipoContato(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, TipoContato> args)
        {
            if (args.Value != null)
            {
                contatoCobrancaDTO.TipoContato = (TipoContatoEnum)args.ItemData.Id;
            }
        }

        private void SelecaoAtendente(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, UsuarioDTO> args)
        {
            if (args.Value != null)
            {
                clienteDTO.UsuarioDTO.IdUsuario = args.ItemData.IdUsuario;
            }
        }

        private void AlteraDtAgenda(Syncfusion.Blazor.Calendars.ChangedEventArgs<DateTime?> args)
        {
            if (args.Value.GetValueOrDefault() < DateTime.Now.Date)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = "Data da agenda não pode ser menor que a data atual";
                mensagemInformativaDTO.Exibe = true;

            }
            else
            {
                DataAgenda = args.Value.GetValueOrDefault();
                contatoCobrancaDTO.DtAgenda = DataAgenda;
            }

        }

        private void AlteraStatusAgenda(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
        {
            if (!args.Checked.Value)
            {
                DataAgenda = DateTime.Now;
                habilitaAgenda = false;
            }
            else
            {
                habilitaAgenda = true;
            }
        }

        private void FechaModalContato()
        {
            exibeModalContato = false;
        }

        private async Task SalvarContato()
        {
            if (indexTipoContato < 0)
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

            foreach (var item in clienteDTO.PagarReceberDTO)
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
            exibeSpinner = false;
            if (!serviceResponse.Success)
            {
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;
            }
            else
            {
                contatoCobrancaDTO.NomeAtendente = Config.usuarioDTO.Nome;
                clienteDTO.ContatoCobrancaDTO.Add(contatoCobrancaDTO);
                exibeModalContato = false;
                mensagemInformativaDTO.Titulo = "Sucesso";
                mensagemInformativaDTO.Mensagem = "Contato gravado com sucesso";
                mensagemInformativaDTO.Exibe = true;
            }
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

            tipoContato = ((TipoContatoEnum[])Enum.GetValues(typeof(TipoContatoEnum))).Select(c => new TipoContato()
            {
                Id = (int)c,
                DsTipoContato = c.ToString()
            }).ToList();

            indexTipoContato = -1;

        }
    }
}
