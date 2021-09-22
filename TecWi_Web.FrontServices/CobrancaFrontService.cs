using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.FrontServices.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.FrontServices
{
    public class CobrancaFrontService : ICobrancaFrontService
    {
        public async Task<ServiceResponse<DateTime>> PopulateData()
        {
            ServiceResponse<DateTime> serviceResponse = new ServiceResponse<DateTime>();
            string url = $"{Config.ApiUrl}PagarReceber/PopulateDataAsync";

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<DateTime>>();

                Config.DtUltAtualicacaoDados = serviceResponse.Data;

            }catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<DateTime>> BuscaDataAtualizacaoDados()
        {
            ServiceResponse<DateTime> serviceResponse = new ServiceResponse<DateTime>();

            string url = $"{Config.ApiUrl}PagarReceber/GetNewestAsync";
            string body = "";
      
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, body);
                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<DateTime>>();

                Config.DtUltAtualicacaoDados = serviceResponse.Data;
            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }
    }
}
