using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Services
{
    public class PagarReceberService : IPagarReceberService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IPagarReceberRepository _iPagarReceberRepository;
        private readonly IClienteRepository _iClienteRepository;
        public PagarReceberService(IMapper iMapper, IUnitOfWork iUnitOfWork, IPagarReceberRepository iPagarReceberRepository, IClienteRepository iClienteRepository)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iPagarReceberRepository = iPagarReceberRepository;
            _iClienteRepository = iClienteRepository;
        }

        public async Task<ServiceResponse<bool>> BulkInsertEfCoreAsync(List<PagarReceberDTO> pagarReceberDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                List<PagarReceber> pagarReceber = _iMapper.Map<List<PagarReceber>>(pagarReceberDTO);
                serviceResponse.Data = await _iPagarReceberRepository.BulkInsertEfCore(pagarReceber);
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> BulkUpdateEfCoreAsync(List<PagarReceberDTO> pagarReceberDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                List<PagarReceber> pagarReceber = _iMapper.Map<List<PagarReceber>>(pagarReceberDTO);
                serviceResponse.Data = _iPagarReceberRepository.BulkUpdateEfCore(pagarReceber);
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public ServiceResponse<List<PagarReceberDTO>> GetAllDapper()
        {
            ServiceResponse<List<PagarReceberDTO>> serviceResponse = new ServiceResponse<List<PagarReceberDTO>>();
            try
            {
                List<PagarReceber> pagarReceber = _iPagarReceberRepository.GetAllDapper();
                List<PagarReceberDTO> pagarReceberDTO = _iMapper.Map<List<PagarReceberDTO>>(pagarReceber);
                serviceResponse.Data = pagarReceberDTO;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> PopulateData()
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                List<PagarReceber> pagarReceber = _iPagarReceberRepository.GetAllDapper();
                List<Cliente> cliente = GetUniqueClients(pagarReceber);
                await _iClienteRepository.BulkInsertAsync(cliente);
                await _iPagarReceberRepository.BulkInsertEfCore(pagarReceber);
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

        private List<Cliente> GetUniqueClients(List<PagarReceber> pagarReceber)
        {
            List<Cliente> cliente = new List<Cliente>();
            foreach (PagarReceber _pagarReceber in pagarReceber.GroupBy(x => x.cdclifor).Select(x => x.First()).ToList())
            {
                cliente.Add(new Cliente
                    (
                        _pagarReceber.cdclifor,
                        _pagarReceber.inscrifed,
                        _pagarReceber.fantasia,
                        _pagarReceber.razao,
                        _pagarReceber.ddd,
                        _pagarReceber.fone1,
                        _pagarReceber.fone2,
                        _pagarReceber.email,
                        _pagarReceber.cidade
                    ));
            }

            return cliente;
        }
    }
}
