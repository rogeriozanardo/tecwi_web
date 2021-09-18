using Newtonsoft.Json;
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

            if(string.IsNullOrEmpty(usuarioDTO.Login) || string.IsNullOrEmpty(usuarioDTO.Senha))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Usuário ou senha não podem ficar em branco.";
                return serviceResponse;
            }

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
                    serviceResponse.Data = await httpResponseMessage.Content.ReadFromJsonAsync<UsuarioDTO>();
                    Config.Autorizado = true;
                    Config.usuarioDTO = new UsuarioDTO();
                    Config.usuarioDTO.UsuarioAplicacaoDTO = new List<UsuarioAplicacaoDTO>();
                    Config.usuarioDTO = serviceResponse.Data;
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
