using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Application.Interfaces.JobsSincronizacao;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.API.HangFireJobs
{
    public class ProdutoJobs
    {
        private readonly IProdutoSincronizacaoService _produtoSincronizacaoService;
        private readonly ILogSincronizacaoProdutoMercoCampService _LogSincronizacaoProdutoMercoCampService;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly IConfiguration _Configuration;
        const string OPCAO_NAO = "0";
        const string OPCAO_SIM = "1";
        //const string CNPJ_MATRIZ = "09615574000491";
        const string CNPJ_MATRIZ = "09615574000149"; //TESTE HOMOLOGACAO
        private DateTime DATA_INICIAL_SINCRONIZACAO = new DateTime(1753, 01, 01);
        const int TAMANHO_MAXIMO_LOTE_ENVIO = 200;

        public ProdutoJobs(IProdutoSincronizacaoService produtoSincronizacaoService,
                           ILogSincronizacaoProdutoMercoCampService logSincronizacaoProdutoMercoCampService,
                           IHttpClientFactory clientFactory,
                           IConfiguration configuration)
        {
            _produtoSincronizacaoService = produtoSincronizacaoService;
            _LogSincronizacaoProdutoMercoCampService = logSincronizacaoProdutoMercoCampService;
            _ClientFactory = clientFactory;
            _Configuration = configuration;
        }

        public async Task SincronizarProdutosAsync()
        {
            await _produtoSincronizacaoService.Sincronizar();
        }

        public async Task EnviarProdutosMercocampAsync()
        {
            var logSincronizacaoProdutomercoCamp = await BuscarUltimaSincronizacaoMercoCamp();
            var produtos = await _produtoSincronizacaoService.BuscarProdutosPorUltimoPeriodoEnviado(logSincronizacaoProdutomercoCamp);

            if (produtos != null && produtos.Any())
            {
                DateTime inicioSincronizacao = DateTime.Now;

                int quantidadeProdutos = produtos.Count();
                int quantidadeEnviada = 0;

                if(quantidadeProdutos > TAMANHO_MAXIMO_LOTE_ENVIO)
                {
                    do
                    {
                        var produtosPaginado = produtos.Skip(quantidadeEnviada).Take
                            (TAMANHO_MAXIMO_LOTE_ENVIO).ToList();
                        await EnviarProdutosMercoCamp(produtosPaginado, logSincronizacaoProdutomercoCamp, inicioSincronizacao);
                        quantidadeEnviada += produtosPaginado.Count();

                    } while (quantidadeEnviada <= quantidadeProdutos);
                }
                else
                    await EnviarProdutosMercoCamp(produtos.ToList(), logSincronizacaoProdutomercoCamp, inicioSincronizacao);
            }
        }

        private async Task<LogSincronizacaoProdutoMercoCamp> BuscarUltimaSincronizacaoMercoCamp()
        {
            return await _LogSincronizacaoProdutoMercoCampService.BuscarUltimaSincronizacao() ??
                    new LogSincronizacaoProdutoMercoCamp
                    {
                        PeriodoInicialEnvio = DATA_INICIAL_SINCRONIZACAO,
                        PeriodoFinalEnvio = DATA_INICIAL_SINCRONIZACAO
                    };
        }

        private LogSincronizacaoProdutoMercoCamp PopularLogSincronizacaoMetroCamp(LogSincronizacaoProdutoMercoCamp logUltimaSincronizacao,
                                                                                  DateTime inicioSincronizacao,
                                                                                  string jsonEnviado)
        {
            var logSincronizacaoProduto = new LogSincronizacaoProdutoMercoCamp
            {
                InicioSincronizacao = inicioSincronizacao,
                PeriodoInicialEnvio = logUltimaSincronizacao.PeriodoFinalEnvio,
                PeriodoFinalEnvio = inicioSincronizacao,
                JsonEnvio = jsonEnviado
            };

            return logSincronizacaoProduto;
        }

        private async Task EnviarProdutosMercoCamp(List<ProdutoMercoCampDTO> produtos, 
                                                   LogSincronizacaoProdutoMercoCamp logUltimaSincronizacao,
                                                   DateTime inicioSincronizacao)
        {
            string jsonEnvio = PopularJson(produtos);
            if (string.IsNullOrEmpty(jsonEnvio))
                return;

            string urlBaseMercoCamp = (string)_Configuration.GetSection("AppSettings").GetValue(typeof(string), "URLBaseMercoCamp");
            var request = new HttpRequestMessage(HttpMethod.Post, urlBaseMercoCamp);

            using (var content = new StringContent(jsonEnvio, System.Text.Encoding.UTF8, "application/json"))
            {
                request.Content = content;
                var client = _ClientFactory.CreateClient();
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                string responseMessage = await response.Content.ReadAsStringAsync();

                if (responseMessage.Contains("CORPEM_WS_ERRO"))
                    throw new Exception(responseMessage);
            }

            var logSincronizacaoMetroCampInserir = PopularLogSincronizacaoMetroCamp(logUltimaSincronizacao, inicioSincronizacao, jsonEnvio);
            await _LogSincronizacaoProdutoMercoCampService.Inserir(logSincronizacaoMetroCampInserir);
        }

        private string PopularJson(List<ProdutoMercoCampDTO> produtos)
        {
            foreach (var produto in produtos)
            {
                produto.IndicadorWS = OPCAO_SIM;
                produto.PoliticaRetiradaMercadoria = OPCAO_NAO;
                produto.DataVencimentoAutomatica = OPCAO_NAO;
                produto.QuantidadeDiasVencimentoAutomatica = OPCAO_NAO;
                produto.ControlaLoteProduto = OPCAO_NAO;
                produto.ControlaDataFabricacaoProduto = OPCAO_NAO;
                produto.ControlaNumeroSerieProduto = OPCAO_NAO;
                produto.ControlaDataVencimentoProduto = OPCAO_NAO;

                foreach (var embalagem in produto.Embalagens)
                {
                    embalagem.PadraoEntrada = OPCAO_SIM;
                    embalagem.PadraoSaida = OPCAO_SIM;
                }
            }

            var envioProdutoMercoCampDTO = new
            {
                CORPEM_ERP_MERC = new
                {
                    CGCCLIWMS = CNPJ_MATRIZ,
                    PRODUTOS = produtos
                },
            };

            var optionSerialize = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Serialize(envioProdutoMercoCampDTO, optionSerialize);
        }
    }
}
