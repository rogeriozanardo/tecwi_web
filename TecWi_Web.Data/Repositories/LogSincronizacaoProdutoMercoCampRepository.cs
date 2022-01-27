using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class LogSincronizacaoProdutoMercoCampRepository : ILogSincronizacaoProdutoMercoCampRepository
    {
        private readonly DataContext _DataContext;

        public LogSincronizacaoProdutoMercoCampRepository(DataContext dataContext)
        {
            _DataContext = dataContext;
        }

        public async Task<LogSincronizacaoProdutoMercoCamp> BuscarUltimaSincronizacaoProduto()
        {
            return await _DataContext.LogSincronizacaoProdutoMercoCamp.OrderByDescending(t => t.ID)
                                                                      .FirstOrDefaultAsync();
        }

        public async Task Inserir(LogSincronizacaoProdutoMercoCamp logSincronizcaoProdutoMercoCamp)
        {
            await _DataContext.LogSincronizacaoProdutoMercoCamp.AddAsync(logSincronizcaoProdutoMercoCamp);
            await _DataContext.SaveChangesAsync();
        }
    }
}
