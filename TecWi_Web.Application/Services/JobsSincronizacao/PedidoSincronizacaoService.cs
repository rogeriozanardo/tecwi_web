using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces.JobsSincronizacao;
using TecWi_Web.Data.Repositories.Sincronizacao.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Services.JobsSincronizacao
{
    public class PedidoSincronizacaoService : IPedidoSincronizacaoService
    {
        private readonly IPedidoSincronizacaoRepository _PedidoSincronizacaoRepository;

        public PedidoSincronizacaoService(IPedidoSincronizacaoRepository pedidoSincronizacaoRepository)
        {
            _PedidoSincronizacaoRepository = pedidoSincronizacaoRepository;
        }

        public async Task Sincronizar()
        {
           await _PedidoSincronizacaoRepository.Sincronizar();
        }


        public async Task AlterarStatusPedidoFaturadoEncerrado()
        {
            await _PedidoSincronizacaoRepository.AlterarStatusPedidoFaturadoEncerrado();
        }

        public async Task<List<PedidoMercoCampDTO>> ListarPedidosSincronizarMercoCamp()
        {
            return await _PedidoSincronizacaoRepository.ListarPedidosNaoEnviadosMercoCamp();
        }
    }
}
