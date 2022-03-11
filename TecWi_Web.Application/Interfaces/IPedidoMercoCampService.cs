using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces
{
    public interface IPedidoMercoCampService
    {
        Task Inserir(PedidoMercoCamp pedidoMercoCamp);

        Task AtualizarStatusPedidosMercoCamp(CORPEM_WMS_CONF_SEP pedidosDTO);
    }
}
