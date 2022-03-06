using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces.JobsSincronizacao;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.API.HangFireJobs
{
    public class MovimentoFiscalJobs
    {
        private readonly IMovimentoFiscalService _MovimentoFiscalService;
        private readonly IConfiguration _Configuration;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly ILogger<MovimentoFiscalJobs> _Logger;

        public MovimentoFiscalJobs(IMovimentoFiscalService movimentoFiscalService,
                                   IConfiguration configuration,
                                   IHttpClientFactory httpClientFactory,
                                   ILogger<MovimentoFiscalJobs> logger)
        {
            _MovimentoFiscalService = movimentoFiscalService;
            _Configuration = configuration;
            _ClientFactory = httpClientFactory;
            _Logger = logger;
        }

        public async Task Sincronizar()
        {
            await _MovimentoFiscalService.Sincronizar();
        }

        public async Task EnviarNotas()
        {
            var movimentosFiscais = await _MovimentoFiscalService.ListarMovimentosFiscaisPendenteTransmissaoMercoCamp();
            string urlBaseMercoCamp = (string)_Configuration.GetSection("AppSettings").GetValue(typeof(string), "URLBaseMercoCamp");

            List<MovimentoFiscal> movimentoFiscalTransmitido = new List<MovimentoFiscal>();
            foreach (var notaFiscal in movimentosFiscais)
            {
                string jsonNotaFiscal = await PopularJsonNotaFiscal(notaFiscal);
                using (var request = new HttpRequestMessage(HttpMethod.Post, urlBaseMercoCamp))
                {
                    using (var content = new StringContent(jsonNotaFiscal, System.Text.Encoding.UTF8, "application/json"))
                    {
                        request.Content = content;
                        var client = _ClientFactory.CreateClient();
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        string responseMessage = await response.Content.ReadAsStringAsync();

                        if (responseMessage.Contains("CORPEM_WS_ERRO"))
                            _Logger.LogError(responseMessage, $"Nota: {notaFiscal.NumNotaFiscal}, Pedido: {notaFiscal.NumPedidoCliente}");
                        else
                        {
                            movimentoFiscalTransmitido.Add(new MovimentoFiscal 
                            { 
                                ID = notaFiscal.ID,
                                Status = StatusTransmissaoMovimentoFiscal.TRANSMITIDO
                            });
                        }
                    }
                }
            }

            if (movimentoFiscalTransmitido.Any())
               await _MovimentoFiscalService.Atualizar(movimentoFiscalTransmitido);
        }

        private async Task<string> PopularJsonNotaFiscal(MovimentoFiscalDTO movimentoFiscalDTO)
        {
            var jsonPedido = new
            {
                CORPEM_ERP_CONF_NF = new
                {
                    CGCCLIWMS = movimentoFiscalDTO.CNPJEmitente ?? string.Empty,
                    CGCEMINF = movimentoFiscalDTO.CNPJEmitente ?? string.Empty,
                    NUMPEDCLI = !string.IsNullOrEmpty(movimentoFiscalDTO.NumPedidoCliente) ? movimentoFiscalDTO.NumPedidoCliente.Trim() : string.Empty,
                    NUMNF = movimentoFiscalDTO.NumNotaFiscal,
                    SERIENF = movimentoFiscalDTO.SerieNotaFiscal,
                    DTEMINF = movimentoFiscalDTO.DataEmissao,
                    VLTOTALNF = movimentoFiscalDTO.ValorTotalNota,
                    QTVOL = movimentoFiscalDTO.ItensMovimentoFiscal.Count(),
                    CHAVENF = movimentoFiscalDTO.ChaveNF,
                    ITENS = movimentoFiscalDTO.ItensMovimentoFiscal
                },
            };

            var optionSerialize = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await Task.FromResult(JsonSerializer.Serialize(jsonPedido, optionSerialize));
        }
    }
}
