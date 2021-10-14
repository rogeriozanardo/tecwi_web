using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Messages;

namespace TecWi_Web.Application.Services
{
    public class UsuarioAplicacaoService : IUsuarioAplicacaoService
    {
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IMapper _iMapper;
        private readonly IUsuarioAplicacaoRepository _iUsuarioAplicacaoRepository;

        public UsuarioAplicacaoService(IUnitOfWork iUnitOfWork, IMapper iMapper, IUsuarioAplicacaoRepository iUsuarioAplicacaoRepository)
        {
            _iUnitOfWork = iUnitOfWork;
            _iMapper = iMapper;
            _iUsuarioAplicacaoRepository = iUsuarioAplicacaoRepository;
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            try
            {
                await DeleteAsync(usuarioAplicacaoDTO[0].IdUsuario);
                serviceResponse = await BulkInsertAsync(usuarioAplicacaoDTO);
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.GetBaseException().Message;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<bool>> BulkInsertAsync(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                List<UsuarioAplicacao> usuarioAplicacao = _iMapper.Map<List<UsuarioAplicacao>>(usuarioAplicacaoDTO);
                serviceResponse.Data = await _iUsuarioAplicacaoRepository.BulkInsert(usuarioAplicacao);
                await _iUnitOfWork.CommitAsync();
                serviceResponse.Message = ServiceMessages.OperacaoConculidaComSucesso;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(Guid idUsuario)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                serviceResponse.Data = await _iUsuarioAplicacaoRepository.Delete(idUsuario);
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UsuarioAplicacaoDTO>>> GetByIdUsuarioAsync(Guid idUsuario)
        {
            ServiceResponse<List<UsuarioAplicacaoDTO>> serviceResponse = new ServiceResponse<List<UsuarioAplicacaoDTO>>();
            try
            {
                List<UsuarioAplicacao> usuarioAplicacao = await _iUsuarioAplicacaoRepository.GetByIdUsuario(idUsuario);
                List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO = _iMapper.Map<List<UsuarioAplicacaoDTO>>(usuarioAplicacao);
                serviceResponse.Data = usuarioAplicacaoDTO;
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Guid>> InsertAsync(UsuarioAplicacaoDTO usuarioAplicacaoDTO)
        {
            ServiceResponse<Guid> serviceResponse = new ServiceResponse<Guid>();
            try
            {
                UsuarioAplicacao usuarioAplicacao = _iMapper.Map<UsuarioAplicacao>(usuarioAplicacaoDTO);
                serviceResponse.Data = await _iUsuarioAplicacaoRepository.Insert(usuarioAplicacao);
                await _iUnitOfWork.CommitAsync();
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
