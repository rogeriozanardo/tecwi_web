using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Application.Services
{
    public class LogSincronizacaoProdutoMercoCampService : ILogSincronizacaoProdutoMercoCampService
    {
        private readonly ILogSincronizacaoProdutoMercoCampRepository _LogSincronizacaoProdutoMercoCampRepository;

        public LogSincronizacaoProdutoMercoCampService(ILogSincronizacaoProdutoMercoCampRepository logSincronizacaoProdutoMercoCampRepository)
        {
            _LogSincronizacaoProdutoMercoCampRepository = logSincronizacaoProdutoMercoCampRepository;
        }

        public async Task<LogSincronizacaoProdutoMercoCamp> BuscarUltimaSincronizacao()
        {
           return await _LogSincronizacaoProdutoMercoCampRepository.BuscarUltimaSincronizacaoProduto();
        }

        public async Task Inserir(LogSincronizacaoProdutoMercoCamp logSincronizacaoProdutoMercoCamp)
        {
            await _LogSincronizacaoProdutoMercoCampRepository.Inserir(logSincronizacaoProdutoMercoCamp);
        }
    }
}
