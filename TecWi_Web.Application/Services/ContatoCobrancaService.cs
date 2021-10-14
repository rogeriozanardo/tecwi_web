using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Services
{
    public class ContatoCobrancaService : IContatoCobrancaService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IContatoCobrancaRepository _iContatoCobrancaRepository;
        private readonly IContatoCobrancaLancamentoRepository _iContatoCobrancaLancamentoRepository;
        public ContatoCobrancaService(IMapper iMapper, IUnitOfWork iUnitOfWork, IContatoCobrancaRepository iContatoCobrancaRepository, IContatoCobrancaLancamentoRepository iContatoCobrancaLancamentoRepository)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iContatoCobrancaRepository = iContatoCobrancaRepository;
            _iContatoCobrancaLancamentoRepository = iContatoCobrancaLancamentoRepository;
        }

        public async Task<ServiceResponse<List<ContatoCobrancaDTO>>> GetAllAsync(ContatoCobrancaFilter contatoCobrancaFilter)
        {
            ServiceResponse<List<ContatoCobrancaDTO>> serviceResponse = new ServiceResponse<List<ContatoCobrancaDTO>>();
            try
            {
                List<ContatoCobranca> contatoCobranca = await _iContatoCobrancaRepository.GetAllAsync(contatoCobrancaFilter);
                List<ContatoCobrancaDTO> contatoCobrancaDTO = _iMapper.Map<List<ContatoCobrancaDTO>>(contatoCobranca);
                serviceResponse.Data = contatoCobrancaDTO;
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
                _iUnitOfWork.Rollback();
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> InsertAsync(ContatoCobrancaDTO contatoCobrancaDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                ContatoCobranca contatoCobranca = _iMapper.Map<ContatoCobranca>(contatoCobrancaDTO);
                contatoCobrancaDTO.IdContato = await _iContatoCobrancaRepository.InsertAsync(contatoCobranca);

                contatoCobrancaDTO.ContatoCobrancaLancamentoDTO.ForEach(x =>
                {
                    x.IdContato = contatoCobrancaDTO.IdContato;
                    x.Idusuario = contatoCobrancaDTO.IdUsuario;
                });
                List<ContatoCobrancaLancamento> contatoCobrancaLancamento = _iMapper.Map<List<ContatoCobrancaLancamento>>(contatoCobrancaDTO.ContatoCobrancaLancamentoDTO);
                await _iContatoCobrancaLancamentoRepository.BulkInsertAsync(contatoCobrancaLancamento);

                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
                _iUnitOfWork.Rollback();
            }
            return serviceResponse;
        }
    }
}
