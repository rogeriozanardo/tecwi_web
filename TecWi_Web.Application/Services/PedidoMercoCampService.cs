using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Services
{
    public class PedidoMercoCampService : IPedidoMercoCampService
    {
        private readonly IPedidoMercoCampRepository _PedidoMercoCampRepository;

        public PedidoMercoCampService(IPedidoMercoCampRepository pedidoMercoCampRepository)
        {
            _PedidoMercoCampRepository = pedidoMercoCampRepository;
        }

        public async Task Inserir(PedidoMercoCamp pedidoMercoCamp)
        {
           await _PedidoMercoCampRepository.Inserir(pedidoMercoCamp);
        }

        public async Task<List<ConfirmacaoPedidoDTO>> ListarPedidosSincronizarTransmitidosMercoCamp()
        {
            return await _PedidoMercoCampRepository.ListarPedidosSincronizarTransmitidosMercoCamp();
        }

        public async Task AtualizarStatusPedidosMercoCamp(List<ConfirmacaoPedidoDTO> pedidosDTO)
        {
            await _PedidoMercoCampRepository.AtualizarStatusPedidosMercoCamp(pedidosDTO);
        }
    }
}
