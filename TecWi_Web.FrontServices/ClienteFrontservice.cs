using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.FrontServices.Interfaces;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.FrontServices
{
    public class ClienteFrontservice : IClienteFrontservice
    {
        public async Task<ServiceResponse<ClienteDTO>> BuscaProximoClienteFilaPorUsuario()
        {
            ServiceResponse<ClienteDTO> serviceResponse = new ServiceResponse<ClienteDTO>();

            ClientePagarReceberFilter clientePagarReceberFilter = new ClientePagarReceberFilter();
            clientePagarReceberFilter.IdUsuario = Config.usuarioDTO.IdUsuario;

            string url = $"{Config.ApiUrl}Cliente/GetNextInQueueAsync";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, clientePagarReceberFilter);

                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<ClienteDTO>>();
            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }
    }
}
