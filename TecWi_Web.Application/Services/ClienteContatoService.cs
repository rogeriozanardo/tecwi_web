using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.UoW;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Services
{
    public class ClienteContatoService : IClienteContatoService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IClienteContatoReposiry _iClienteContatoReposiry;
        public ClienteContatoService(IMapper iMapper, IUnitOfWork iUnitOfWork, IClienteContatoReposiry iClienteContatoReposiry)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iClienteContatoReposiry = iClienteContatoReposiry;
        }
        public async Task<ServiceResponse<bool>> DeleteAsync(Guid IdClienteContato)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                bool result = await _iClienteContatoReposiry.DeleteAsync(IdClienteContato);
                await _iUnitOfWork.CommitAsync();
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ClienteContatoDTO>>> GetByClienteAsync(int Cdclifor)
        {
            ServiceResponse<List<ClienteContatoDTO>> serviceResponse = new ServiceResponse<List<ClienteContatoDTO>>();
            try
            {
                List<ClienteContato> clienteContato = await _iClienteContatoReposiry.GetByClienteAsync(Cdclifor);
                List<ClienteContatoDTO> clienteContatoDTO = _iMapper.Map<List<ClienteContatoDTO>>(clienteContato);
                await _iUnitOfWork.CommitAsync();
                serviceResponse.Data = clienteContatoDTO;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Guid>> InsertAsync(ClienteContatoDTO clienteContatoDTO)
        {
            ServiceResponse<Guid> serviceResponse = new ServiceResponse<Guid>();
            try
            {
                ClienteContato clienteContato = new ClienteContato();
                clienteContato.Cdclifor = clienteContatoDTO.Cdclifor;
                clienteContato.Email = clienteContatoDTO.Email;
                clienteContato.IdClienteContato = Guid.NewGuid();
                clienteContato.Nome = clienteContatoDTO.Nome;
                clienteContato.Telefone = clienteContatoDTO.Telefone;

                Guid resul = await _iClienteContatoReposiry.InsertAsync(clienteContato);
                await _iUnitOfWork.CommitAsync();
                serviceResponse.Data = resul;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Guid>> UpdateAsync(ClienteContatoDTO clienteContatoDTO)
        {
            ServiceResponse<Guid> serviceResponse = new ServiceResponse<Guid>();
            try
            {
                ClienteContato clienteContato = await  _iClienteContatoReposiry.GetByIdContato(clienteContatoDTO.IdClienteContato);
                if(clienteContato is not null)
                {
                    clienteContato.Nome = clienteContatoDTO.Nome;
                    clienteContato.Telefone = clienteContatoDTO.Telefone;
                    clienteContato.Email = clienteContatoDTO.Email;

                    Guid resul = await _iClienteContatoReposiry.UpdateAsync(clienteContato);
                    await _iUnitOfWork.CommitAsync();
                    serviceResponse.Data = resul;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Problema ao atualizar a pessoa de contato.";
                }
                                
                
                
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