﻿using AutoMapper;
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
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Guid>> InsertAsync(ContatoCobrancaDTO contatoCobrancaDTO)
        {
            ServiceResponse<Guid> serviceResponse = new ServiceResponse<Guid>();
            try
            {
                ContatoCobranca contatoCobranca = _iMapper.Map<ContatoCobranca>(contatoCobrancaDTO);
                contatoCobrancaDTO.IdContato = await _iContatoCobrancaRepository.InsertAsync(contatoCobranca);

                contatoCobrancaDTO.ContatoCobrancaLancamentoDTO.ForEach(x => { x.IdContato = contatoCobrancaDTO.IdContato; });
                List<ContatoCobrancaLancamento> contatoCobrancaLancamento = _iMapper.Map<List<ContatoCobrancaLancamento>>(contatoCobrancaDTO.ContatoCobrancaLancamentoDTO);
                await _iContatoCobrancaLancamentoRepository.BulkInsertAsync(contatoCobrancaLancamento);

                serviceResponse.Data = contatoCobrancaDTO.IdContato;
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
