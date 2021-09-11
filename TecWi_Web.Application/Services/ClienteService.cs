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
    public class ClienteService : IClienteService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IClienteRepository _iClienteRepository;
        public ClienteService(IMapper iMapper, IUnitOfWork iUnitOfWork, IClienteRepository iClienteRepository)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iClienteRepository = iClienteRepository;
        }

        public async Task<ServiceResponse<bool>> BulkInsertOrUpdateAsync(List<ClienteDTO> clienteDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                List<Cliente> cliente = _iMapper.Map<List<Cliente>>(clienteDTO);
                await _iClienteRepository.BulkInsertAsync(cliente);
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ClienteDTO>>> GetAllAsync(ClientePagarReceberFilter clientePagarReceberFilter)
        {
            ServiceResponse<List<ClienteDTO>> serviceResponse = new ServiceResponse<List<ClienteDTO>>();
            try
            {
                List<Cliente> cliente = await _iClienteRepository.GetAllAsync(clientePagarReceberFilter);
                List<ClienteDTO> clienteDTO = _iMapper.Map<List<ClienteDTO>>(cliente);
                serviceResponse.Data = clienteDTO;
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
