using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ClienteService : IClienteService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IClienteRepository _iClienteRepository;
        private readonly IUsuarioRepository _iUsuarioRepository;

        public ClienteService(IMapper iMapper, IUnitOfWork iUnitOfWork, IClienteRepository iClienteRepository, IUsuarioRepository iUsuarioRepository)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iClienteRepository = iClienteRepository;
            _iUsuarioRepository = iUsuarioRepository;
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

                foreach (var item in clienteDTO)
                {
                    item.totalLancamentos = item.PagarReceberDTO.Sum(x => x.valorr);
                    foreach (var lancamento in item.PagarReceberDTO)
                    {
                        lancamento.qtdDiasVencido = DateTime.Now.Subtract(lancamento.dtvencto).Days;
                    }
                }

                serviceResponse.Data = clienteDTO;
                await _iUnitOfWork.CommitAsync();
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
                if(cliente != null)
                {
                    ClienteDTO clienteDTO = _iMapper.Map<ClienteDTO>(cliente);

                    clienteDTO.totalLancamentos = clienteDTO.PagarReceberDTO.Sum(x => x.valorr);

                    foreach (var item in clienteDTO.PagarReceberDTO)
                    {
                        item.qtdDiasVencido = DateTime.Now.Subtract(item.dtvencto).Days;
                    }

                    serviceResponse.Data = clienteDTO;
                    await _iUnitOfWork.CommitAsync();
                }
                
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(ClienteDTO clienteDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                Cliente cliente = _iMapper.Map<Cliente>(clienteDTO);
                cliente.IdUsuario = clienteDTO.UsuarioDTO.IdUsuario;
                await _iClienteRepository.UpdateAsync(cliente);
                await _iUnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.GetBaseException().Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> AtualizaBaseClientesByReceber(List<PagarReceber> pagarReceber)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            try
            {
                List<Cliente> clientesMatriz = new List<Cliente>();

                var clientesZ4 = await _iClienteRepository.BuscaListaClienteTotalZ4();

                clientesMatriz = pagarReceber.GroupBy(p => new
                {
                    p.Cdclifor,
                    p.Cdfilial,
                    p.Cidade,
                    p.DDD,
                    p.Email,
                    p.Fantasia,
                    p.Fone1,
                    p.Fone2,
                    p.Inscrifed,
                    p.Razao
                }).Select(c => new Cliente()
                {
                    Cdclifor = c.First().Cdclifor,
                    Cidade = c.First().Cidade,
                    DDD = c.First().DDD,
                    Email = c.First().Email,
                    Fantasia = c.First().Fantasia,
                    Fone1 = c.First().Fone1,
                    Fone2 = c.First().Fone2,
                    Razao = c.First().Razao
                }).ToList();
                
                foreach (var item in clientesZ4)
                {
                     clientesMatriz.RemoveAll(x => x.Cdclifor == item.Cdclifor);
                }

                if (clientesMatriz.Count == 0)
                {
                    return serviceResponse;
                }

                List<Usuario> usuario = await GetUsuarioWithPermission();

                AssingUsuarioToClienteRandonly(clientesMatriz, usuario);

                await _iClienteRepository.InsereCliente(clientesMatriz);

            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.GetBaseException().Message;
            }

            return serviceResponse;
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

        private async Task<List<Usuario>> GetUsuarioWithPermission()
        {
            List<Usuario> usuario = await _iUsuarioRepository.GetAllAsync(new UsuarioFilter { });
            return usuario.Where(x => x.UsuarioAplicacao.Any(y => y.IdAplicacao == IdAplicacao.Cobranca && y.IdPerfil == IdPerfil.Operador && y.StAtivo)).ToList();
        }
    }
}
