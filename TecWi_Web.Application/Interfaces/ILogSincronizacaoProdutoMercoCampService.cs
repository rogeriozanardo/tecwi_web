using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Application.Interfaces
{
    public interface ILogSincronizacaoProdutoMercoCampService
    {
        Task Inserir(LogSincronizacaoProdutoMercoCamp logSincronizacaoProdutoMercoCamp);

        Task<LogSincronizacaoProdutoMercoCamp> BuscarUltimaSincronizacao();
    }
}
