using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Application.Interfaces
{
    public interface IPedidoMercoCampService
    {
        Task Inserir(PedidoMercoCamp pedidoMercoCamp);
    }
}
