using System;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using Microsoft.EntityFrameworkCore;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;
using System.Collections.Generic;
using TecWi_Web.Shared.Filters;
using System.Linq;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Data.Repositories
{
    public class LogOperacaoRepository : ILogOperacaoRepository
    {
        private readonly DataContext _dataContext;

        public LogOperacaoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task ClearLogOperacaoAsync(int OlderThanInDays)
        {
            List<LogOperacao> logOperacao = await _dataContext.Set<LogOperacao>().Where(x => x.Data <= DateTime.Now.AddDays(-OlderThanInDays)).ToListAsync();
            _dataContext.Set<LogOperacao>().RemoveRange(logOperacao);
        }

        public async Task<List<LogOperacao>> GetLogOperacaoAsync(LogOperacaoFilter logOperacaoFilter)
        {

            IQueryable<LogOperacao> _logOperacao = _dataContext.LogOperacao
                .Include(x => x.Usuario)
                .Where(x => logOperacaoFilter.IdUsuario != Guid.Empty ? x.IdUsuario == logOperacaoFilter.IdUsuario : true)
                .Where(x => Enum.IsDefined(typeof(TipoOperacao), (int)logOperacaoFilter.tipoOperacao) ? x.TipoOperacao == logOperacaoFilter.tipoOperacao : true)
                .Where(x => logOperacaoFilter.DataStart != null ? x.Data >= (DateTime)logOperacaoFilter.DataStart : true)
                .Where(x => logOperacaoFilter.DataEnd != null ? x.Data <= (DateTime)logOperacaoFilter.DataEnd : true);

            List<LogOperacao> logOperacao = await _logOperacao
                .OrderByDescending(x => x.Data)
                .AsNoTracking()
                .Skip((logOperacaoFilter.Page - 1) * logOperacaoFilter.PageSize)
                .Take(logOperacaoFilter.PageSize)
                .ToListAsync();

            return logOperacao;
        }

        public async Task<Guid> InsertAsync(LogOperacao logOperacao)
        {
            
            await _dataContext.LogOperacao.AddAsync(logOperacao);
            await _dataContext.SaveChangesAsync();
            return logOperacao.IdLogOperacao;
        }
    }
}
