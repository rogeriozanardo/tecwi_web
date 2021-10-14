using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Services
{
    public class LogOperacaoService : ILogOperacaoService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IHttpContextAccessor _ihttpContextAccessor;
        private readonly ILogOperacaoRepository _iLogOperacaoRepository;

        public LogOperacaoService(IMapper iMapper, IUnitOfWork iUnitOfWork, IHttpContextAccessor iHttpContextAccessor, ILogOperacaoRepository iLogOperacaoRepository)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _ihttpContextAccessor = iHttpContextAccessor;
            _iLogOperacaoRepository = iLogOperacaoRepository;
        }

        private Guid GetUserId() => Guid.Parse(_ihttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<bool>> ClearLogOperacaoAsync(int OlderThanInDays)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                await _iLogOperacaoRepository.ClearLogOperacaoAsync(OlderThanInDays);
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<LogOperacaoDTO>>> GetAllAsync(LogOperacaoFilter logOperacaoFilter)
        {
            ServiceResponse<List<LogOperacaoDTO>> serviceResponse = new ServiceResponse<List<LogOperacaoDTO>>();
            try
            {
                List<LogOperacao> logOperacao = await _iLogOperacaoRepository.GetLogOperacaoAsync(logOperacaoFilter);
                List<LogOperacaoDTO> logOperacaoDTO = _iMapper.Map<List<LogOperacaoDTO>>(logOperacao);
                serviceResponse.Data = logOperacaoDTO;
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<LogOperacaoDTO>> GetNewestAsync(TipoOperacao tipoOperacao)
        {
            ServiceResponse<LogOperacaoDTO> serviceResponse = new ServiceResponse<LogOperacaoDTO>();
            try
            {
                LogOperacaoFilter logOperacaoFilter = new LogOperacaoFilter
                {
                    PageSize = 1,
                    tipoOperacao = tipoOperacao
                };

                List<LogOperacao> logOperacao = await _iLogOperacaoRepository.GetLogOperacaoAsync(logOperacaoFilter);
                LogOperacao _logOperacao = logOperacao.FirstOrDefault();
                if (_logOperacao != null)
                {
                    LogOperacaoDTO logOperacaoDTO = _iMapper.Map<LogOperacaoDTO>(_logOperacao);
                    serviceResponse.Data = logOperacaoDTO;
                }

                await _iUnitOfWork.CommitAsync();

            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Guid>> InsertAsync(LogOperacaoDTO LogOperacaoDTO)
        {
            ServiceResponse<Guid> serviceResponse = new ServiceResponse<Guid>();
            try
            {
                LogOperacao logOperacao = new LogOperacao(Guid.NewGuid(), GetUserId(), LogOperacaoDTO.TipoOperacao, LogOperacaoDTO.Data);
                await _iLogOperacaoRepository.InsertAsync(logOperacao);
                await _iUnitOfWork.CommitAsync();

                serviceResponse.Data = logOperacao.IdLogOperacao;
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
