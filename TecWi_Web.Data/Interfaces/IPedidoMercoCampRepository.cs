using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface IPedidoMercoCampRepository
    {
        Task Inserir(PedidoMercoCamp pedidoMercoCamp);
    }
}
