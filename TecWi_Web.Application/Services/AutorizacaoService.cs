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
using TecWi_Web.Shared.DTOs;
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

        public async Task<ServiceResponse<string>> Login(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();
            try
            {
                Usuario usuario = await _iUsuarioRepository.GetByEmailAsync(usuarioDTO.Email);
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
                    serviceResponse.Data = CreateTokem(usuario);
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

                Usuario usuario = new Usuario
                {
                    IdUsuario = new Guid(),
                    Login = usuarioDTO.Login,
                    Nome = usuarioDTO.Nome,
                    Email = usuarioDTO.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                await _iUsuarioRepository.Insert(usuario);

                List<UsuarioAplicacao> usuarioAplicacao = _iMapper.Map<List<UsuarioAplicacao>>(usuarioDTO.UsuarioAplicacaoDTO);
                usuarioAplicacao.ForEach(x => x.IdUsuario = usuario.IdUsuario);
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

        private async Task<bool> UsuarioExists(UsuarioDTO usuarioDTO)
        {
            Usuario usuario = await _iUsuarioRepository.GetByEmailAsync(usuarioDTO.Email);
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
