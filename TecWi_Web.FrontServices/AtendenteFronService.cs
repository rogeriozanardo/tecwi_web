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
    public class AtendenteFronService : IAtendenteFronService
    {
        public async Task<ServiceResponse<List<AtendenteDTO>>> ListaPerformanceAtendentes(PesquisaDTO pesquisaDTO)
        {
            ServiceResponse<List<AtendenteDTO>> serviceResponse = new ServiceResponse<List<AtendenteDTO>>();
            string url = $"{Config.ApiUrl}Atendente/ListaPerformanceAtendentes";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, pesquisaDTO);

                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<List<AtendenteDTO>>>();
            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.GetBaseException().Message;
            }
            return serviceResponse;
        }
    }
}
