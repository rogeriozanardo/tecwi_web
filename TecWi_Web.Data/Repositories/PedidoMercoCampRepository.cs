using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class PedidoMercoCampRepository : IPedidoMercoCampRepository
    {
        private readonly DataContext _DataContext;

        public PedidoMercoCampRepository(DataContext dataContext)
        {
            _DataContext = dataContext;
        }

        public async Task Inserir(PedidoMercoCamp pedidoMercoCamp)
        {
           await _DataContext.PedidoMercoCamp.AddAsync(pedidoMercoCamp);
           await _DataContext.SaveChangesAsync();
        }
    }
}
