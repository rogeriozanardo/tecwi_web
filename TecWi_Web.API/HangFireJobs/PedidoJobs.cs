using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Application.Interfaces.JobsSincronizacao;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.API.HangFireJobs
{
    public class PedidoJobs
    {
        private readonly IPedidoSincronizacaoService _PedidoSincronizacaoService;
        private readonly IPedidoMercoCampService _PedidoMercoCampService;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly IConfiguration _Configuration;

        public PedidoJobs(IPedidoSincronizacaoService pedidoSincronizacaoService,
                          IPedidoMercoCampService pedidoMercoCampService,
                          IHttpClientFactory clientFactory,
                          IConfiguration configuration)
        {
            _PedidoSincronizacaoService = pedidoSincronizacaoService;
            _ClientFactory = clientFactory;
            _Configuration = configuration;
            _PedidoMercoCampService = pedidoMercoCampService;
        }

        public async Task SincronizarPedidosAsync()
        {
            await _PedidoSincronizacaoService.Sincronizar();
        }

        public async Task EnviarPedidosMercoCampAsync()
        {
            var pedidos = await _PedidoSincronizacaoService.ListarPedidosSincronizarMercoCamp();
            var cnpj = BuscarCNPJ("CNPJ_Matriz");
            string urlBaseMercoCamp = (string)_Configuration.GetSection("AppSettings").GetValue(typeof(string), "URLBaseMercoCamp");
          
            foreach (var pedido in pedidos)
            {
                string jsonPedido = PopularJson(pedido, cnpj);
                var request = new HttpRequestMessage(HttpMethod.Post, urlBaseMercoCamp);

                using (var content = new StringContent(jsonPedido, System.Text.Encoding.UTF8, "application/json"))
                {
                    request.Content = content;
                    var client = _ClientFactory.CreateClient();
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    string responseMessage = await response.Content.ReadAsStringAsync();

                    if (responseMessage.Contains("CORPEM_WS_ERRO"))
                        throw new Exception(responseMessage);
                }

                var pedidoMercoCamp = PopularPedidoMercoCamp(pedido);
                await _PedidoMercoCampService.Inserir(pedidoMercoCamp);
            }
        }

        public async Task AlterarStatusPedidoFaturadoParaEncerradoAsync()
        {
            await _PedidoSincronizacaoService.AlterarStatusPedidoFaturadoEncerrado();
        }

        private string BuscarCNPJ(string cnpj)
        {
            return (string)_Configuration.GetSection("AppSettings").GetValue(typeof(string),cnpj); 
        }

        private string PopularJson(PedidoMercoCampDTO pedido, string cnpj)
        {
            var jsonPedido = new
            {
                CORPEM_ERP_DOC_SAI = new
                {
                    CGCCLIWMS = cnpj,
                    CGCEMINF = pedido.CNPJEmitente,
                    OBSPED = pedido.ObservacaoPedido,
                    OBSROM = pedido.ObservacaoRomaneio,
                    NUMPEDCLI = pedido.NumeroPedidoCliente,
                    NUMPEDRCA = pedido.NumeroPedidoRCA,
                    VLTOTPED = pedido.ValorTotalPedido,
                    CGCDEST = pedido.CNPJDestinatario,
                    NOMEDEST = pedido.NomeDestinatario,
                    CEPDEST = pedido.CepDestinatario,
                    UFDEST = pedido.UFDestinatario,
                    IBGEMUNDEST = pedido.IBGEMunicipioDestinatario,
                    MUN_DEST = pedido.MunicipioDestinatario,
                    BAIR_DEST = pedido.BairroDestinatario,
                    LOGR_DEST = pedido.LogradouroDestinatario,
                    NUM_DEST = pedido.NumeroDestinatario,
                    COMP_DEST = pedido.ComplementoDestinatario,
                    TP_FRETE = pedido.TipoFrete,
                    CODVENDEDOR = pedido.CodigoVendedor,
                    NOMEVENDEDOR = pedido.NomeVendedor,
                    DTINCLUSAOERP = pedido.DataInclusaoERP,
                    DTLIBERACAOERP = pedido.DataLiberacaoERP,
                    DTPREV_ENT_SITE = pedido.DataPrevisaoEntradaSite,
                    EMAILRASTRO = pedido.EmailRastro,
                    DDDRASTRO = pedido.DDDRastro,
                    TELRASTRO = pedido.TelRastro,
                    CGC_TRP = pedido.CNPJTransportadora,
                    UF_TRP = pedido.UFTransportadora,
                    PRIORIDADE = pedido.Prioridade,
                    ETQCLIZPLBASE64 = pedido.Etiqueta,
                    ITENS = pedido.Itens
                },
            };

            var optionSerialize = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Serialize(jsonPedido, optionSerialize);
        }

        private PedidoMercoCamp PopularPedidoMercoCamp(PedidoMercoCampDTO pedido)
        {
            var pedidoMercoCamp = new PedidoMercoCamp()
            {
                DataEnvio = DateTime.Now,
                NumPedido = Convert.ToInt32(pedido.NumeroPedidoCliente),
                Peso = decimal.Zero,
                Volume = 0,
                SeqTransmissao = pedido.SequenciaEnvio,
                Status = StatusPedidoMercoCamp.Transmitido
            };

            foreach (var item in pedido.Itens)
            {
                pedidoMercoCamp.PedidoItens.Add(new PedidoItemMercoCamp
                {
                    CdProduto = item.CodigoProduto,
                    NumPedido = Convert.ToInt32(pedido.NumeroPedidoCliente),
                    PedidoMercoCampId = pedidoMercoCamp.IdPedidoMercoCamp,
                    PedidoMercoCamp = pedidoMercoCamp,
                    Qtd = item.Quantidade,
                    QtdSeparado = 0,
                    SeqTransmissao = item.Sequencia
                });
            }

            return pedidoMercoCamp;
        }
    }
}
