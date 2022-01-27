using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface ILogSincronizacaoProdutoMercoCampRepository
    {
        Task<LogSincronizacaoProdutoMercoCamp> BuscarUltimaSincronizacaoProduto();
        Task Inserir(LogSincronizacaoProdutoMercoCamp logSincronizcaoProdutoMercoCamp);
    }
}
