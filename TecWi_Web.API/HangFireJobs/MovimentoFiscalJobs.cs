﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
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
            var movimentosFiscais = await _MovimentoFiscalService.ListarMovimentosFiscaisPendenteTransmissaoMercoCamp() 
                                          ?? new List<MovimentoFiscalDTO>();

            if (!movimentosFiscais.Any())
                return;

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
            string pdfDanfe = await BuscarDanfePedido(movimentoFiscalDTO.ChaveNF);
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
                    DANFEPDFBASE64 = pdfDanfe,
                    ITENS = movimentoFiscalDTO.ItensMovimentoFiscal
                },
            };

            var optionSerialize = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await Task.FromResult(JsonSerializer.Serialize(jsonPedido, optionSerialize));
        }

        private async Task<string> BuscarDanfePedido(string chaveNF)
        {
            string caminhoArquivosDanfe = (string)_Configuration.GetSection("AppSettings").GetValue(typeof(string), "CAMINHO_ARQUIVO_DANFE");
            if(!Directory.Exists(caminhoArquivosDanfe))
                return string.Empty;

           var arquivosXml = Directory.GetFiles(caminhoArquivosDanfe);
            if (arquivosXml.Count() == 0)
                return string.Empty;

           var caminhoArquivoChaveNF = string.Concat(caminhoArquivosDanfe, "\\", chaveNF);
           var xmlPorChaveNF = arquivosXml.FirstOrDefault(t => t.StartsWith(caminhoArquivoChaveNF));
            if (string.IsNullOrEmpty(xmlPorChaveNF))
                return string.Empty;

            try
            {
                Document document = new Document();
                MemoryStream ms = new MemoryStream();

                // iTextSharp
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                //MyPageEvents pageEvents = new MyPageEvents();
                //writer.PageEvent = pageEvents;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPorChaveNF);

                StringReader sr = new StringReader(xmlDoc.InnerXml);
                XmlTextReader reader = new XmlTextReader(sr);
                ITextHandler xmlHandler = new ITextHandler(document);

                try
                {
                    xmlHandler.Parse(reader);
                }
                catch (Exception e)
                {
                    ms.Close();
                }
                finally
                {
                    reader.Close();
                    sr.Close();
                }

                byte[] teste = null;
                //using (var msTeste = new MemoryStream())
                //{
                //    BinaryWriter bw = new BinaryWriter(msTeste);
                //    bw.Write(ms.ToArray());
                //    bw.Close();
                //    teste = msTeste.ToArray();
                //    ms.Close();
                //}

                FileStream fs = new FileStream(@"C:\Users\eliton\source\repos\RogerioZanardo\tecwi_web\TecWi_Web.API\bin\Debug\net5.0\teste.pdf", FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(ms.ToArray());
                bw.Close();
                fs.Close();
                ms.Close();


                return await Task.FromResult(Convert.ToBase64String(teste));

                //using (var fileStream = new FileStream(xmlPorChaveNF, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                //    using (PdfDocument pdfDocument = new PdfDocument())
                //    {
                //        pdfDocument.LoadFromStream(fileStream);

                //        using (var memoryStream = new MemoryStream())
                //        {
                //            pdfDocument.SaveToStream(memoryStream, FileFormat.PDF);
                //            byte[] pdf = memoryStream.ToArray();
                //            return await Task.FromResult(Convert.ToBase64String(pdf));
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
