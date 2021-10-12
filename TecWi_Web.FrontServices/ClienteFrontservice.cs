using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.FrontServices.Interfaces;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;
using TecWi_Web.Shared.Utility;

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

        public async Task<ServiceResponse<List<ClienteDTO>>> PesquisaCliente(string pesquisa)
        {
            ServiceResponse<List<ClienteDTO>> serviceResponse = new ServiceResponse<List<ClienteDTO>>();
            ClientePagarReceberFilter clientePagarReceberFilter = new ClientePagarReceberFilter();

            string url = $"{Config.ApiUrl}Cliente/GetAllAsync";

            bool somenteNumeros = Utilitarios.ChecaSeStringTemSomenteNumeros(pesquisa);
            if(somenteNumeros)
            {
                clientePagarReceberFilter.inscrifed = pesquisa;
            }else
            {
                clientePagarReceberFilter.fantasia = pesquisa;
                clientePagarReceberFilter.razao = pesquisa;
            }

            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, clientePagarReceberFilter);

                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<List<ClienteDTO>>>();
            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> SalvaCliente(ClienteDTO clienteDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            string url = $"{Config.ApiUrl}Cliente/BulkInsertOrUpdateAsyncIndividual";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, clienteDTO);

                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.GetBaseException().Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<Guid>> SalvaContatoCliente(ClienteContatoDTO clienteContatoDTO)
        {
            ServiceResponse<Guid> serviceResponse = new ServiceResponse<Guid>();
            string url = string.Empty;
            
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);

            try
            {
                if(clienteContatoDTO.IdClienteContato.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    url = $"{Config.ApiUrl}ClienteContato/InsertAsync";
                    httpResponseMessage = await httpClient.PostAsJsonAsync(url, clienteContatoDTO);
                }
                else
                {
                    url = $"{Config.ApiUrl}ClienteContato/UpdateAsync";
                    httpResponseMessage = await httpClient.PutAsJsonAsync(url, clienteContatoDTO);
                }

                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<Guid>>();
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
