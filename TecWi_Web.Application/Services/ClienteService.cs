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
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IClienteRepository _iClienteRepository;
        private readonly IHttpContextAccessor _ihttpContextAccessor;
        public ClienteService(IMapper iMapper, IUnitOfWork iUnitOfWork, IClienteRepository iClienteRepository, IHttpContextAccessor iHttpContextAccessor)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iClienteRepository = iClienteRepository;
            _ihttpContextAccessor = iHttpContextAccessor;
        }

        private Guid GetUserId() => Guid.Parse(_ihttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

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
                clientePagarReceberFilter.IdUsuario = GetUserId();
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

        public async Task<ServiceResponse<ClienteDTO>> GetNextInQueueAsync(ClientePagarReceberFilter clientePagarReceberFilter)
        {
            ServiceResponse<ClienteDTO> serviceResponse = new ServiceResponse<ClienteDTO>();
            try
            {
                Cliente cliente = await _iClienteRepository.GetNextInQueueAsync(clientePagarReceberFilter);
                ClienteDTO clienteDTO = _iMapper.Map<ClienteDTO>(cliente);

                clienteDTO.totalLancamentos = clienteDTO.PagarReceberDTO.Sum(x => x.valorr);

                foreach(var item in clienteDTO.PagarReceberDTO)
                {
                    item.qtdDiasVencido = DateTime.Now.Subtract(item.dtvencto).Days;
                }

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
