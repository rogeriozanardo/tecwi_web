using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;
using TecWi_Web.Shared.Utility;

namespace TecWi_Web.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IUsuarioRepository _iUsuarioRepository;
        private readonly IUsuarioAplicacaoRepository _iUsuarioAplicacaoRepository;

        public UsuarioService(IMapper iMapper, IUnitOfWork iUnitOfWork, IUsuarioRepository iUsuarioRepository, IUsuarioAplicacaoRepository iUsuarioAplicacaoRepository)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iUsuarioRepository = iUsuarioRepository;
            _iUsuarioAplicacaoRepository = iUsuarioAplicacaoRepository;
        }

        public async Task<ServiceResponse<List<UsuarioDTO>>> GetAllAsync(UsuarioFilter usuarioFilter)
        {
            ServiceResponse<List<UsuarioDTO>> serviceResponse = new ServiceResponse<List<UsuarioDTO>>();
            try
            {
                List<Usuario> usuario = await _iUsuarioRepository.GetAllAsync(usuarioFilter);
                List<UsuarioDTO> usuarioDTO = _iMapper.Map<List<UsuarioDTO>>(usuario);

                serviceResponse.Data = usuarioDTO;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<UsuarioDTO>> GetByIdAsync(Guid Idusuario)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = new ServiceResponse<UsuarioDTO>();
            try
            {
                Usuario usuario = await _iUsuarioRepository.GetByIdAsync(Idusuario);
                UsuarioDTO usuarioDTO = _iMapper.Map<UsuarioDTO>(usuario);
                serviceResponse.Data = usuarioDTO;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<UsuarioDTO>> GetByLoginAsync(string Login)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = new ServiceResponse<UsuarioDTO>();
            try
            {
                Usuario usuario = await _iUsuarioRepository.GetByLoginAsync(Login);
                UsuarioDTO usuarioDTO = _iMapper.Map<UsuarioDTO>(usuario);
                serviceResponse.Data = usuarioDTO;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> InsertAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                Usuario checaUsuario = await _iUsuarioRepository.GetByLoginAsync(usuarioDTO.Login);
                if(checaUsuario != null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Login já cadastrado.";
                    return serviceResponse;
                }


                PasswordHashUtitlity.CreatePaswordHash(usuarioDTO.Senha, out byte[] senhaHash, out byte[] senhaSalt);
                Usuario usuario = new Usuario(Guid.NewGuid(), usuarioDTO.Login, usuarioDTO.Nome, usuarioDTO.Email, senhaHash, senhaSalt);
                await _iUsuarioRepository.Insert(usuario);

                List<UsuarioAplicacao> usuarioAplicacao = new List<UsuarioAplicacao>();

                foreach(var item in usuarioDTO.UsuarioAplicacaoDTO)
                {
                    usuarioAplicacao.Add(new UsuarioAplicacao()
                    {
                        IdUsuario = usuario.IdUsuario,
                        IdAplicacao = item.IdAplicacao,
                        IdPerfil = item.IdPerfil,
                        StAtivo = item.StAtivo

                    });
                }

                await _iUsuarioAplicacaoRepository.BulkInsert(usuarioAplicacao);
                await _iUnitOfWork.CommitAsync();
                
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                PasswordHashUtitlity.CreatePaswordHash(usuarioDTO.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                Usuario usuario = await _iUsuarioRepository.GetByIdAsync(usuarioDTO.IdUsuario);
                usuario.Update(usuarioDTO.IdUsuario, usuarioDTO.Login, usuarioDTO.Nome, usuarioDTO.Email, senhaHash, senhaSalt);
                
                _iUsuarioRepository.Update(usuario);

                await _iUnitOfWork.CommitAsync();
                serviceResponse.Data = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateJustInfoAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                Usuario usuario = await _iUsuarioRepository.GetByIdAsync(usuarioDTO.IdUsuario);
                usuario.Update(usuarioDTO.IdUsuario, usuarioDTO.Login, usuarioDTO.Nome, usuarioDTO.Email, usuario.SenhaHash, usuario.SenhaSalt);
                usuario.Ativo = usuarioDTO.Ativo;
                _iUsuarioRepository.Update(usuario);

                await _iUnitOfWork.CommitAsync();
                serviceResponse.Data = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }
    }
}
