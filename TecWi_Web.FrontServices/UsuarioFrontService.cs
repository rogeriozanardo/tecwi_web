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
using TecWi_Web.Shared.Filters;

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
                string url = $"{Config.ApiUrl}Autorizacao/LoginAsync";
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
                    Config.usuarioDTO.Token = $"Bearer {serviceResponse.Data.Token}";
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
        public async Task<ServiceResponse<bool>> UpdateJustInfoAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            try
            {
                if (string.IsNullOrEmpty(usuarioDTO.Login))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Informe o Login do usuário.";
                    return serviceResponse;
                }

                if (string.IsNullOrEmpty(usuarioDTO.Nome))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Informe o Nome do usuário.";
                    return serviceResponse;
                }

                string url = $"{Config.ApiUrl}Usuario/UpdateJustInfoAsync";

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, usuarioDTO);
                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<bool>> SalvarUsuario(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            string url = string.Empty;
            try
            {
                var validaUsuario = ValidaCadastroUsuario(usuarioDTO);
                if(!validaUsuario.Success)
                {
                    return validaUsuario;
                }

                if(usuarioDTO.IdUsuario.ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    url = $"{Config.ApiUrl}Usuario/InsertAsync";
                }
                else
                {
                    url = $"{Config.ApiUrl}Usuario/UpdateAsync";
                }

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, usuarioDTO);
                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        private ServiceResponse<bool> ValidaCadastroUsuario(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            if (string.IsNullOrEmpty(usuarioDTO.Login))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Informe o Login do usuário.";
            }

            if (string.IsNullOrEmpty(usuarioDTO.Nome))
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Informe o Nome do usuário.";
            }

            if (usuarioDTO.Senha.Length < 6 || usuarioDTO.Senha.Length > 15)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "A senha deve ter no mínimo 6 caracteres e no máximo 15 caracteres.";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UsuarioDTO>>> GetAllAsync(UsuarioFilter usuarioFilter)
        {
            ServiceResponse<List<UsuarioDTO>> serviceResponse = new ServiceResponse<List<UsuarioDTO>>();
            string url = $"{Config.ApiUrl}Usuario/GetAllAsync";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, usuarioFilter);

                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<List<UsuarioDTO>>>();

            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> AtualizaAplicacoesUsuario(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            string url = $"{Config.ApiUrl}UsuarioAplicacao/UpdateAsync";
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", Config.usuarioDTO.Token);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync(url, usuarioAplicacaoDTO);
                serviceResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
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
