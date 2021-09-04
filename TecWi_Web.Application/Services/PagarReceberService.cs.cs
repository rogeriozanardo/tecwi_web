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

        public async Task<ServiceResponse<List<PagarReceberDTO>>> GetAllDapper()
        {
            ServiceResponse<List<PagarReceberDTO>> serviceResponse = new ServiceResponse<List<PagarReceberDTO>>();
            try
            {
                List<PagarReceber> pagarReceber = await _iPagarReceberRepository.GetPenddingPagarReceber();
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
                List<PagarReceber> pagarReceberDapper = await _iPagarReceberRepository.GetPenddingPagarReceber();
                List<PagarReceber> pagarReceberEfCore = await _iPagarReceberRepository.GetAllEfCore();
                List<PagarReceber> PagarReceberDifference = pagarReceberEfCore.Where(x => !pagarReceberDapper.Any(y => y.SeqID == x.SeqID && y.Stcobranca == x.Stcobranca && y.Numlancto == x.Numlancto)).ToList();
                PagarReceberDifference.ForEach(x => { x.Stcobranca = false; });
                _iPagarReceberRepository.BulkUpdateEfCore(PagarReceberDifference);

                List<Cliente> cliente = GetUniqueClients(pagarReceberDapper);
                await _iClienteRepository.BulkInsertAsync(cliente);
                await _iPagarReceberRepository.BulkInsertEfCore(pagarReceberDapper);
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
            foreach (PagarReceber _pagarReceber in pagarReceber.GroupBy(x => x.Cdclifor).Select(x => x.First()).ToList())
            {
                cliente.Add(new Cliente
                    (
                        _pagarReceber.Cdclifor,
                        _pagarReceber.Inscrifed,
                        _pagarReceber.Fantasia,
                        _pagarReceber.Razao,
                        _pagarReceber.DDD,
                        _pagarReceber.Fone1,
                        _pagarReceber.Fone2,
                        _pagarReceber.Email,
                        _pagarReceber.Cidade
                    ));
            }

            return cliente;
        }
    }
}
