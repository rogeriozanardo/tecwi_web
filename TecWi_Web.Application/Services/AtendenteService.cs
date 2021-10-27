using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Services
{
    public class AtendenteService : IAtendenteService
    {
        private readonly IAtendenteRepository _atendenteRepository;

        public AtendenteService(IAtendenteRepository atendenteRepository)
        {
            _atendenteRepository = atendenteRepository;
        }
        public async Task<ServiceResponse<List<AtendenteDTO>>> ListaPerformanceAtendentes(PesquisaDTO pesquisaDTO)
        {
            ServiceResponse<List<AtendenteDTO>> serviceResponse = new ServiceResponse<List<AtendenteDTO>>();
            try
            {
                var listaAtendimentos = await _atendenteRepository.ListaAtendimentosDetalhados(pesquisaDTO);

                serviceResponse.Data = listaAtendimentos.GroupBy(l => new { l.IdUsuario, l.NomeUsuario }).Select(u => new AtendenteDTO()
                {
                    IdUsuario = u.First().IdUsuario,
                    Nome = u.First().NomeUsuario
                }).ToList();

                foreach (var atendente in serviceResponse.Data)
                {
                    atendente.clienteDTO = listaAtendimentos.Where(x => x.IdUsuario == atendente.IdUsuario).GroupBy(l => new { l.Cdclifor, l.Razao }).Select(c => new ClienteDTO()
                    {
                        IdUsuario = atendente.IdUsuario,
                        cdclifor = c.First().Cdclifor,
                        razao = c.First().Razao
                    }).ToList();
                }

                foreach (var atendente in serviceResponse.Data)
                {
                    foreach (var cliente in atendente.clienteDTO)
                    {
                        cliente.ContatoCobrancaDTO = listaAtendimentos.Where(x => x.Cdclifor == cliente.cdclifor && x.IdUsuario == atendente.IdUsuario)
                            .Select(c => new ContatoCobrancaDTO()
                            {
                                IdUsuario = c.IdUsuario,
                                Cdclifor = c.Cdclifor,
                                IdContato = c.IdContato,
                                DtContato = c.DtContato,
                                TipoContato = c.TipoContato,
                                Anotacao = c.Anotacao
                            }).ToList();

                        cliente.totalContatos = cliente.ContatoCobrancaDTO.Count();

                        atendente.TotalContatos += cliente.ContatoCobrancaDTO.Count();
                    }
                }
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.GetBaseException().Message;
            }
            return serviceResponse;
        }
    }
}
