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
using TecWi_Web.Shared.Messages;

namespace TecWi_Web.Application.Services
{
    public class PagarReceberService : IPagarReceberService
    {
        private readonly IMapper _iMapper;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IPagarReceberRepository _iPagarReceberRepository;
        private readonly IClienteRepository _iClienteRepository;
        private readonly IUsuarioRepository _iUsuarioRepository;
        private readonly ILogOperacaoRepository _iLogOperacaoRepository;
        private readonly IHttpContextAccessor _iHttpContextAccessor;
        private readonly IClienteService _iClienteService;
        public PagarReceberService(IMapper iMapper, IUnitOfWork iUnitOfWork, IPagarReceberRepository iPagarReceberRepository, IClienteRepository iClienteRepository, IUsuarioRepository iUsuarioRepository, ILogOperacaoRepository iLogOperacaoRepository, IHttpContextAccessor iHttpContextAccessor, IClienteService iClienteService)
        {
            _iMapper = iMapper;
            _iUnitOfWork = iUnitOfWork;
            _iPagarReceberRepository = iPagarReceberRepository;
            _iClienteRepository = iClienteRepository;
            _iUsuarioRepository = iUsuarioRepository;
            _iLogOperacaoRepository = iLogOperacaoRepository;
            _iHttpContextAccessor = iHttpContextAccessor;
            _iClienteService = iClienteService;
        }

        private Guid GetUserId() => Guid.Parse(_iHttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

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

        public async Task<ServiceResponse<DateTime>> PopulateDataAsync()
        {
            ServiceResponse<DateTime> serviceResponse = new ServiceResponse<DateTime>();
            try
            {
                bool hasValidUsers = await ValidadeUsuarioAplicacao();
                if (hasValidUsers)
                {
                    List<PagarReceber> pagarReceberPending = await _iPagarReceberRepository.GetPendingPagarReceber();
                    List<PagarReceber> pagarReceberPaid = await _iPagarReceberRepository.GetPaidPagarReceber();
                    List<PagarReceber> pagarReceberEfCore = await _iPagarReceberRepository.GetAllEfCore(new PagarReceberFilter { Stcobranca = true });
                    List<Cliente> clienteEFCore = await _iClienteRepository.GetAllAsync(new ClientePagarReceberFilter { IdUsuario = Guid.Empty });

                    await RemoveDependenciesToUpdate(clienteEFCore);
                    await InsertCliente(pagarReceberPending, clienteEFCore);
                    await UpdateCliente(pagarReceberPending, clienteEFCore);
                    await InsertPagarReceber(pagarReceberEfCore, pagarReceberPending);
                    await UpdatePagarReceberDifferent(pagarReceberEfCore, pagarReceberPending);
                    await UpdatePagarReceberPaid(pagarReceberPaid);
                    await InsertLog();

                    await _iUnitOfWork.CommitAsync();
                    serviceResponse.Data = DateTime.Now;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = ServiceMessages.NaoExistemUsuariosComAcessoAGetaoDeCobranca;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.GetBaseException().Message;
            }
            return serviceResponse;
        }

        private async Task<bool> ValidadeUsuarioAplicacao()
        {

            List<Usuario> usuario = await GetUsuarioWithPermission();
            return usuario.Count != 0;
        }

        private async Task<List<Usuario>> GetUsuarioWithPermission()
        {
            List<Usuario> usuario = await _iUsuarioRepository.GetAllAsync(new UsuarioFilter { });
            return usuario.Where(x => x.UsuarioAplicacao.Any(y => y.IdAplicacao == IdAplicacao.Cobranca && y.IdPerfil == IdPerfil.Operador && y.StAtivo)).ToList();
        }

        private async Task RemoveDependenciesToUpdate(List<Cliente> clienteEFCore)
        {
            await Task.Run(() =>
            {
                clienteEFCore.ForEach(x =>
                {
                    x.Usuario = null;
                    x.PagarReceber = null;
                    x.ContatoCobranca = null;
                    x.ClienteContato = null;
                });
            });
        }

        private async Task InsertCliente(List<PagarReceber> pagarReceberPending, List<Cliente> clienteEFCore)
        {
            List<Cliente> cliente = GetUniqueClients(pagarReceberPending);
            List<Cliente> clienteToInsert = cliente.Where(x => !clienteEFCore.Any(y => y.Cdclifor == x.Cdclifor)).ToList();
            List<Usuario> usuario = await GetUsuarioWithPermission();

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

                _clienteEfCore.Update(_cliente.Cdclifor, _cliente.Inscrifed, _cliente.Fantasia, _cliente.Razao, _cliente.DDD, _cliente.Fone1, _cliente.Fone2, _cliente.Email, _cliente.Cidade); ;
                clienteToUpdate.Add(_clienteEfCore);
            }

            await _iClienteRepository.BulkUpdateAsync(clienteToUpdate);
        }

        private async Task InsertPagarReceber(List<PagarReceber> pagarReceberEfCore, List<PagarReceber> pagarReceberPending)
        {
            List<PagarReceber> PagarReceberToInsert = pagarReceberPending.Where(x => !pagarReceberEfCore.Where(y => y.SeqID == x.SeqID && y.Numlancto == x.Numlancto).Any()).ToList();
            if (PagarReceberToInsert.Count > 0)
            {
                await _iPagarReceberRepository.BulkInsertEfCore(PagarReceberToInsert);
            }
        }

        private async Task UpdatePagarReceberDifferent(List<PagarReceber> pagarReceberEfCore, List<PagarReceber> pagarReceberPending)
        {
            List<PagarReceber> PagarReceberToUpdate = pagarReceberEfCore.Where(x => !pagarReceberPending.Any(y => y.SeqID == x.SeqID && y.Numlancto == x.Numlancto)).ToList();
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

        private async Task InsertLog()
        {
            LogOperacao logOperacao = new LogOperacao
            (
                Guid.NewGuid(),
                GetUserId(),
                TipoOperacao.AtualizarDadosCobranca,
                DateTime.Now
            );

            await _iLogOperacaoRepository.InsertAsync(logOperacao);
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

        public async Task<ServiceResponse<DateTime>> AtualizarPagarReceber()
        {
            ServiceResponse<DateTime> serviceResponse = new ServiceResponse<DateTime>();
            List<PagarReceber> pagarReceberSymphony = new List<PagarReceber>();
            List<PagarReceber> pagarReceberZ4 = new List<PagarReceber>();
            try
            {
                pagarReceberZ4 = await _iPagarReceberRepository.BuscaListaReceberZ4();

                pagarReceberSymphony = await _iPagarReceberRepository.BuscaListaReceberSymphony();

                var processaClientes = await _iClienteService.AtualizaBaseClientesByReceber(pagarReceberSymphony);

                if(!processaClientes.Success)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = processaClientes.Message;
                    return serviceResponse;
                }
              

                foreach (var item in pagarReceberSymphony)
                {
                    int index = pagarReceberZ4.FindIndex(x => x.SeqID == item.SeqID && x.Cdfilial.Trim() == item.Cdfilial.Trim());
                    if(index < 0)
                    {
                        await _iPagarReceberRepository.InsereReceber(item);
                    }
                }

                foreach(var item in pagarReceberZ4)
                {
                    int index = pagarReceberSymphony.FindIndex(x => x.SeqID == item.SeqID && x.Cdfilial.Trim() == item.Cdfilial.Trim());
                    if(index < 0)
                    {
                        await _iPagarReceberRepository.ExcluiReceber(item);
                    }
                }

                await InsertLog();
                serviceResponse.Data = DateTime.Now;
            }
            catch(Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.GetBaseException().Message;
            }

            return serviceResponse;
        }
    }
}