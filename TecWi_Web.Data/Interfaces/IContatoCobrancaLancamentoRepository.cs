using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Interfaces
{
    public interface IContatoCobrancaLancamentoRepository
    {
        Task BulkInsertAsync(List<ContatoCobrancaLancamento> contatoCobrancaLancamento);

        Task<List<ContatoCobrancaLancamento>> GetAllAsyc(ContatoCobrancaLancamentoFilter contatoCobrancaLancamento);
    }
}
