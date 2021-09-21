using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Interfaces
{
    public interface ILogOperacaoRepository
    {
        Task<Guid> InsertAsync(LogOperacao logOperacao);

        Task<List<LogOperacao>> GetLogOperacaoAsync(LogOperacaoFilter logOperacaoFilter);

        Task ClearLogOperacaoAsync(int OlderThanInDays);
    }
}
