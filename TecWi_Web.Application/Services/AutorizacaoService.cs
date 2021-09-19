using AutoMapper;
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
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;
using TecWi_Web.Shared.Messages;
using TecWi_Web.Shared.Utility;

namespace TecWi_Web.Application.Services
{
    public class AutorizacaoService : IAutorizacaoService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IMapper _iMapper;
        private readonly IUsuarioRepository _iUsuarioRepository;
        private readonly IUsuarioAplicacaoRepository _iUsuarioAplicacaoRepository;

        public AutorizacaoService(IConfiguration configuration, IUnitOfWork iUnitOfWork, IMapper iMapper, IUsuarioRepository iUsuarioRepository, IUsuarioAplicacaoRepository iUsuarioAplicacaoRepository)
        {
            _configuration = configuration;
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _iUsuarioRepository = iUsuarioRepository;
            _iUsuarioAplicacaoRepository = iUsuarioAplicacaoRepository;
        }

        public async Task<ServiceResponse<UsuarioDTO>> Login(UsuarioDTO usuarioDTO)
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
                    else
                    {
                        // implementar aqui a busca pela lista de aplicações e perfis do usuário
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

        public async Task<ServiceResponse<UsuarioDTO>> Register(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = new ServiceResponse<UsuarioDTO>();
            try
            {
                if (await UsuarioExists(usuarioDTO))
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = ServiceMessages.UsuarioJaCadastrado;
                    return serviceResponse;
                }

                PasswordHashUtitlity.CreatePaswordHash(usuarioDTO.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                Usuario usuario = new Usuario(new Guid(), usuarioDTO.Login, usuarioDTO.Nome, usuarioDTO.Email, senhaHash, senhaSalt);

                await _iUsuarioRepository.Insert(usuario);

                usuarioDTO.UsuarioAplicacaoDTO.ForEach(x => { x.IdUsuario = usuario.IdUsuario; });
                List<UsuarioAplicacao> usuarioAplicacao = _iMapper.Map<List<UsuarioAplicacao>>(usuarioDTO.UsuarioAplicacaoDTO);
                await _iUsuarioAplicacaoRepository.BulkInsert(usuarioAplicacao);

                await _iUnitOfWork.CommitAsync();

                serviceResponse.Data = usuarioDTO;
                serviceResponse.Message = ServiceMessages.UsuarioSucessoCadastro;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UsuarioDTO>>> GetUserList(UsuarioFilter usuarioFilter)
        {
            ServiceResponse<List<UsuarioDTO>> serviceResponse = new ServiceResponse<List<UsuarioDTO>>();
            
            try
            {
                List<Usuario> usuarioList = new List<Usuario>();
                usuarioList = await _iUsuarioRepository.GetAllAsync(usuarioFilter);

                serviceResponse.Data = _iMapper.Map<List<UsuarioDTO>>(usuarioList);

            }catch(Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }
        private async Task<bool> UsuarioExists(UsuarioDTO usuarioDTO)
        {
            Usuario usuario = await _iUsuarioRepository.GetByLoginAsync(usuarioDTO.Login);
            return usuario != null;
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
