﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Services
{
    public class PagarReceberService : IPagarReceberService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IPagarReceberRepository _iPagarReceberRepository;
        private readonly IClienteRepository _iClienteRepository;
        private readonly IUsuarioRepository _iUsuarioRepository;
        public PagarReceberService(IMapper iMapper, IUnitOfWork iUnitOfWork, IPagarReceberRepository iPagarReceberRepository, IClienteRepository iClienteRepository, IUsuarioRepository iUsuarioRepository)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iPagarReceberRepository = iPagarReceberRepository;
            _iClienteRepository = iClienteRepository;
            _iUsuarioRepository = iUsuarioRepository;
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
                serviceResponse.Data = await _iPagarReceberRepository.BulkUpdateEfCore(pagarReceber);
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
                List<PagarReceber> pagarReceber = await _iPagarReceberRepository.GetPendingPagarReceber();
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
                Task<List<PagarReceber>> TaskPagarReceberPending = _iPagarReceberRepository.GetPendingPagarReceber();
                Task<List<PagarReceber>> TaskPagarReceberPaid = _iPagarReceberRepository.GetPaidPagarReceber();
                Task<List<PagarReceber>> TaskPagarReceberEfCore = _iPagarReceberRepository.GetAllEfCore(new PagarReceberFilter { Stcobranca = true });
                Task<List<Cliente>> TaskClienteEfCore = _iClienteRepository.GetAllAsync(new ClientePagarReceberFilter { }, Guid.Empty);

                await Task.WhenAll(TaskPagarReceberPending, TaskPagarReceberPaid, TaskPagarReceberEfCore, TaskClienteEfCore);

                List<PagarReceber> pagarReceberPending = TaskPagarReceberPending.Result;
                List<PagarReceber> pagarReceberPaid = TaskPagarReceberPaid.Result;
                List<PagarReceber> pagarReceberEfCore = TaskPagarReceberEfCore.Result;
                List<Cliente> clienteEFCore = TaskClienteEfCore.Result;

                await InsertCliente(pagarReceberPending, clienteEFCore);
                await UpdateCliente(pagarReceberPending, clienteEFCore);
                await InsertPagarReceber(pagarReceberEfCore, pagarReceberPending);
                await UpdatePagarReceberDifferent(pagarReceberEfCore, pagarReceberPending);
                await UpdatePagarReceberPaid(pagarReceberPaid);

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

        private async Task InsertCliente(List<PagarReceber> pagarReceberPending, List<Cliente> clienteEFCore)
        {
            List<Cliente> cliente = GetUniqueClients(pagarReceberPending);
            List<Cliente> clienteToInsert = cliente.Where(x => !clienteEFCore.Any(y => y.Cdclifor == x.Cdclifor)).ToList();
            List<Usuario> usuario = await _iUsuarioRepository.GetAllAsync(new UsuarioFilter());

            AssingUsuarioToClienteRandonly(clienteToInsert, usuario);

            if (clienteToInsert.Count > 0)
            {
                await _iClienteRepository.BulkInsertAsync(clienteToInsert);
            }
        }

        private async Task UpdateCliente(List<PagarReceber> pagarReceberPending, List<Cliente> clienteEFCore)
        {
            List<Cliente> cliente = GetUniqueClients(pagarReceberPending);
            List<Cliente> clienteEfCore = cliente.Where(x => clienteEFCore.Any(y => y.Cdclifor == x.Cdclifor)).ToList();
            List<Cliente> clienteToUpdate = new List<Cliente>();
            foreach (Cliente _cliente in clienteEfCore)
            {
                Cliente _clienteEfCore = clienteEFCore.FirstOrDefault(x => x.Cdclifor == _cliente.Cdclifor);
                if (_clienteEfCore == null)
                {
                    continue;
                }

                _clienteEfCore.Update(_cliente.Cdclifor,  _cliente.Inscrifed, _cliente.Fantasia, _cliente.Razao, _cliente.DDD, _cliente.Fone1, _cliente.Fone2, _cliente.Email, _cliente.Cidade); ;
                clienteToUpdate.Add(_clienteEfCore);
            }

            await _iClienteRepository.BulkUpdateAsync(clienteToUpdate);
        }

        private async Task InsertPagarReceber(List<PagarReceber> pagarReceberEfCore, List<PagarReceber> pagarReceberPending)
        {
            List<PagarReceber> PagarReceberToInsert = pagarReceberPending.Where(x => !pagarReceberEfCore.Any(y => y.SeqID == x.SeqID && y.Stcobranca == x.Stcobranca && y.Numlancto == x.Numlancto)).ToList();
            if (PagarReceberToInsert.Count > 0)
            {
                await _iPagarReceberRepository.BulkInsertEfCore(PagarReceberToInsert);
            }
        }

        private async Task UpdatePagarReceberDifferent(List<PagarReceber> pagarReceberEfCore, List<PagarReceber> pagarReceberPending)
        {
            List<PagarReceber> PagarReceberToUpdate = pagarReceberEfCore.Where(x => !pagarReceberPending.Any(y => y.SeqID == x.SeqID && y.Stcobranca == x.Stcobranca && y.Numlancto == x.Numlancto)).ToList();
            PagarReceberToUpdate.ForEach(x =>
             {
                 x.Stcobranca = false;
             });

            if (PagarReceberToUpdate.Count > 0)
            {
                await _iPagarReceberRepository.BulkUpdateEfCore(PagarReceberToUpdate);
            }
        }

        private async Task UpdatePagarReceberPaid(List<PagarReceber> pagarReceberPaid)
        {
            List<PagarReceber> pagarReceberUpdate = new List<PagarReceber>();
            foreach (PagarReceber _pagarReceber in pagarReceberPaid)
            {
                PagarReceberFilter pagarReceberFilter = new PagarReceberFilter { SeqID = _pagarReceber.SeqID, Numlancto = _pagarReceber.Numlancto, Sq = _pagarReceber.Sq };
                PagarReceber pagarReceberToUpdate = await _iPagarReceberRepository.GetPagarReceber(pagarReceberFilter);
                if (pagarReceberToUpdate != null)
                {
                    pagarReceberToUpdate.Update(false, _pagarReceber.Dtpagto.Value);
                    pagarReceberUpdate.Add(await _iPagarReceberRepository.GetPagarReceber(pagarReceberFilter));
                }
            }

            if (pagarReceberUpdate.Count > 0)
            {
                await _iPagarReceberRepository.BulkUpdateEfCore(pagarReceberUpdate);
            }
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

        public void AssingUsuarioToClienteRandonly(List<Cliente> cliente, List<Usuario> usuario)
        {
            Random random = new Random();
            List<Usuario> usuarioTemp = new List<Usuario>();

            foreach (Cliente _cliente in cliente)
            {
                if (usuarioTemp.Count == 0)
                {
                    usuarioTemp.AddRange(usuario);
                }

                Usuario _usuarioTemp = usuarioTemp.OrderBy(x => random.Next()).FirstOrDefault();
                _cliente.IdUsuario = _usuarioTemp.IdUsuario;
                usuarioTemp.Remove(_usuarioTemp);
            }
        }
    }
}