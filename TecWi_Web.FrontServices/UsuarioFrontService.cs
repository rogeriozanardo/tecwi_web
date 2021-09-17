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

namespace TecWi_Web.FrontServices
{
    public class UsuarioFrontService : IUsuarioFrontService
    {
        public async Task<ServiceResponse<UsuarioDTO>> Login(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = new ServiceResponse<UsuarioDTO>();
            try
            {
                string url = $"{Config.ApiUrl}Autorizacao/Login";
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, usuarioDTO);

                if(httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<UsuarioDTO>>();
                    Config.Autorizado = false;
                }
                else
                {
                    
                }
            }
            catch(Exception e)
            {
                Config.Autorizado = false;
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }
    }
}
