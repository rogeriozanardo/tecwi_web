using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces.JobsSincronizacao
{
    public interface IProdutoSincronizacaoService
    {
        Task Sincronizar();

        Task<IEnumerable<ProdutoMercoCampDTO>> BuscarProdutosPorUltimoPeriodoEnviado(LogSincronizacaoProdutoMercoCamp logSincronicaoProdutoMercoCamp);
    }
}
