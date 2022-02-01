using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces.JobsSincronizacao
{
    public interface IPedidoSincronizacaoService
    {
        Task Sincronizar();
        Task AlterarStatusPedidoFaturadoEncerrado();
        Task<List<PedidoMercoCampDTO>> ListarPedidosSincronizarMercoCamp();
    }
}
