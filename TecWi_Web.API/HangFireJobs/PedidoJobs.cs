using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
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
            string urlBaseMercoCamp = (string)_Configuration.GetSection("AppSettings").GetValue(typeof(string), "URLBaseMercoCamp");

            foreach(var pedido in pedidos)
            {
                string jsonPedido = await PopularJson(pedido);
                using (var request = new HttpRequestMessage(HttpMethod.Post, urlBaseMercoCamp))
                {
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
                }

                var pedidoMercoCamp = await PopularPedidoMercoCamp(pedido);
                await _PedidoMercoCampService.Inserir(pedidoMercoCamp);
            }
        }

        public async Task AlterarStatusPedidoFaturadoParaEncerradoAsync()
        {
            await _PedidoSincronizacaoService.AlterarStatusPedidoFaturadoEncerrado();
        }

        private async Task<string> PopularJson(PedidoMercoCampDTO pedido)
        {
            var jsonPedido = new
            {
                CORPEM_ERP_DOC_SAI = new
                {
                    CGCCLIWMS = pedido.CNPJEmitente ?? string.Empty,
                    CGCEMINF = pedido.CNPJEmitente ?? string.Empty,
                    OBSPED = pedido.ObservacaoPedido ?? string.Empty,
                    OBSROM = pedido.ObservacaoRomaneio ?? string.Empty,
                    NUMPEDCLI = !string.IsNullOrEmpty(pedido.NumeroPedidoCliente) ? pedido.NumeroPedidoCliente.Trim() : string.Empty,
                    NUMPEDRCA = !string.IsNullOrEmpty(pedido.NumeroPedidoRCA) ? pedido.NumeroPedidoRCA.Trim() : string.Empty,
                    VLTOTPED = pedido.ValorTotalPedido.ToString(),
                    CGCDEST = pedido.CNPJDestinatario ?? string.Empty,
                    NOMEDEST = !string.IsNullOrEmpty(pedido.NomeDestinatario) ? pedido.NomeDestinatario.Trim() : string.Empty,
                    CEPDEST = !string.IsNullOrEmpty(pedido.CepDestinatario) ? pedido.CepDestinatario.Trim() : string.Empty,
                    UFDEST = !string.IsNullOrEmpty(pedido.UFDestinatario) ? pedido.UFDestinatario.Trim() : string.Empty,
                    IBGEMUNDEST = pedido.IBGEMunicipioDestinatario ?? string.Empty,
                    MUN_DEST = !string.IsNullOrEmpty(pedido.MunicipioDestinatario) ? pedido.MunicipioDestinatario.Trim() : string.Empty,
                    BAIR_DEST = !string.IsNullOrEmpty(pedido.BairroDestinatario) ? pedido.BairroDestinatario.Trim() : string.Empty,
                    LOGR_DEST = !string.IsNullOrEmpty(pedido.LogradouroDestinatario) ? pedido.LogradouroDestinatario.Trim() : string.Empty,
                    NUM_DEST = !string.IsNullOrEmpty(pedido.NumeroDestinatario) ? pedido.NumeroDestinatario.Trim() : string.Empty,
                    COMP_DEST = !string.IsNullOrEmpty(pedido.ComplementoDestinatario) ? pedido.ComplementoDestinatario.Trim() : string.Empty,
                    TP_FRETE = pedido.TipoFrete,
                    CODVENDEDOR = !string.IsNullOrEmpty(pedido.CodigoVendedor) ? pedido.CodigoVendedor.Trim() : string.Empty,
                    NOMEVENDEDOR = !string.IsNullOrEmpty(pedido.NomeVendedor) ? pedido.NomeVendedor.Trim() : string.Empty,
                    DTINCLUSAOERP = pedido.DataInclusaoERP.ToString("dd/MM/yyyy"),
                    DTLIBERACAOERP = pedido.DataLiberacaoERP.ToString("dd/MM/yyyy"),
                    DTPREV_ENT_SITE = pedido.DataPrevisaoEntradaSite.ToString("dd/MM/yyyy"),
                    EMAILRASTRO = !string.IsNullOrEmpty(pedido.EmailRastro) ? pedido.EmailRastro.Trim() : string.Empty,
                    DDDRASTRO = !string.IsNullOrEmpty(pedido.DDDRastro) ? pedido.DDDRastro.Trim() : string.Empty,
                    TELRASTRO = !string.IsNullOrEmpty(pedido.TelRastro) ? pedido.TelRastro.Trim() : string.Empty,
                    CGC_TRP = !string.IsNullOrEmpty(pedido.CNPJTransportadora) ? pedido.CNPJTransportadora.Trim() : string.Empty,
                    UF_TRP = !string.IsNullOrEmpty(pedido.UFTransportadora) ? pedido.UFTransportadora.Trim() : string.Empty,
                    PRIORIDADE = string.Empty,
                    ETQCLIZPLBASE64 = string.Empty,
                    ITENS = pedido.Itens
                },
            };

            var optionSerialize = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await Task.FromResult(JsonSerializer.Serialize(jsonPedido, optionSerialize));
        }

        private async Task<PedidoMercoCamp> PopularPedidoMercoCamp(PedidoMercoCampDTO pedido)
        {
            var pedidoMercoCamp = new PedidoMercoCamp()
            {
                DataEnvio = DateTime.Now,
                NumPedido = Convert.ToInt32(pedido.NumeroPedidoCliente),
                Peso = pedido.Itens.Sum(x => x.Peso.GetValueOrDefault(decimal.Zero)),
                Volume = pedido.Itens.GroupBy(t => t.CodigoProduto).Select(t => t).Distinct().Count(),
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
                    Qtd = string.IsNullOrEmpty(item.Quantidade) ? decimal.Zero : Convert.ToDecimal(item.Quantidade),
                    QtdSeparado = 0,
                    SeqTransmissao = string.IsNullOrEmpty(item.Sequencia) ? Convert.ToInt16(item.Sequencia) : 0
                });
            }

            return await Task.FromResult(pedidoMercoCamp);
        }
    }
}
