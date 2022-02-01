using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Data.Repositories.Sincronizacao.Interfaces
{
    public interface IPedidoSincronizacaoRepository
    {
        Task Sincronizar();
        Task AlterarStatusPedidoFaturadoEncerrado();
        Task<List<PedidoMercoCampDTO>> ListarPedidosNaoEnviadosMercoCamp();
    }
}
