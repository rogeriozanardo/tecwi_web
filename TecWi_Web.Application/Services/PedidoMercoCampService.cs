using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

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
    }
}
