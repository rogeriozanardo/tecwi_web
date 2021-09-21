﻿using AutoMapper;
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

        public UsuarioService(IMapper iMapper, IUnitOfWork iUnitOfWork, IUsuarioRepository iUsuarioRepository)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iUsuarioRepository = iUsuarioRepository;
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

        public async Task<ServiceResponse<Guid>> InsertAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<Guid> serviceResponse = new ServiceResponse<Guid>();
            try
            {
                PasswordHashUtitlity.CreatePaswordHash(usuarioDTO.Senha, out byte[] senhaHash, out byte[] senhaSalt);
                Usuario usuario = new Usuario(Guid.NewGuid(), usuarioDTO.Login, usuarioDTO.Nome, usuarioDTO.Email, senhaHash, senhaSalt);
                await _iUsuarioRepository.Insert(usuario);
                await _iUnitOfWork.CommitAsync();
                serviceResponse.Data = usuario.IdUsuario;
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
    }
}
