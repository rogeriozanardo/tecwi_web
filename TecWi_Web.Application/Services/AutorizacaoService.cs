using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Messages;

namespace TecWi_Web.Application.Services
{
    public class AutorizacaoService : IAutorizacaoService
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _iUsuarioRepository;

        public AutorizacaoService(IConfiguration configuration, IUsuarioRepository iUsuarioRepository)
        {
            _configuration = configuration;
            _iUsuarioRepository = iUsuarioRepository;
        }

        public async Task<ServiceResponse<UsuarioDTO>> LoginAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = new ServiceResponse<UsuarioDTO>();
            serviceResponse.Data = new UsuarioDTO();
            serviceResponse.Data.UsuarioAplicacaoDTO = new List<UsuarioAplicacaoDTO>();

            try
            {
                Usuario usuario = await _iUsuarioRepository.GetByLoginAsync(usuarioDTO.Login);
                if (usuario == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = ServiceMessages.UsuarioNaoEncontrado;

                }
                else if (!VerifyPasswordHash(usuarioDTO.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = ServiceMessages.UsuarioSenhaInvalida;
                }
                else
                {
                    serviceResponse.Data.Login = usuario.Login;
                    serviceResponse.Data.Nome = usuario.Nome;
                    serviceResponse.Data.Email = usuario.Email;
                    serviceResponse.Data.Token = CreateTokem(usuario);

                    if(usuario.Login == "admin")
                    {
                        serviceResponse.Data.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Cobranca, IdPerfil = IdPerfil.Gestor });
                        serviceResponse.Data.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Comercial, IdPerfil = IdPerfil.Gestor });
                        serviceResponse.Data.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Configuracoes, IdPerfil = IdPerfil.Gestor });
                        serviceResponse.Data.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Financeiro, IdPerfil = IdPerfil.Gestor });
                        serviceResponse.Data.UsuarioAplicacaoDTO.Add(new UsuarioAplicacaoDTO() { IdAplicacao = IdAplicacao.Logistica, IdPerfil = IdPerfil.Gestor });
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        private bool VerifyPasswordHash(string senha, byte[] senhaHash, byte[] senhadSalt)
        {
            using (var hmacsha = new System.Security.Cryptography.HMACSHA512(senhadSalt))
            {
                var ComputeHash = hmacsha.ComputeHash(Encoding.UTF8.GetBytes(senha));
                for (int i = 0; i < ComputeHash.Length; i++)
                {
                    if (ComputeHash[i] != senhaHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string CreateTokem(Usuario usuario)
        {
            List<Claim> claim = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Login),
            };

            SymmetricSecurityKey systemSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
            );

            SigningCredentials signingCredentials = new SigningCredentials(systemSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}
