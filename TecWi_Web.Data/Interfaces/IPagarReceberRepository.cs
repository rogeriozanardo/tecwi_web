using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Interfaces
{
    public interface IPagarReceberRepository
    {
        Task<List<PagarReceber>> GetPendingPagarReceber();
        Task<List<PagarReceber>> GetPaidPagarReceber();
        Task<List<PagarReceber>> GetAllEfCore(PagarReceberFilter pagarReceberFilter);
        Task<PagarReceber> GetPagarReceber(PagarReceberFilter pagarReceberFilter);
        Task<bool> BulkUpdateEfCore(List<PagarReceber> pagarReceber);
        Task<bool> BulkInsertEfCore(List<PagarReceber> pagarReceber);
        Task<List<PagarReceber>> BuscaListaReceberSymphony();
        Task<List<PagarReceber>> BuscaListaReceberZ4();
        Task<bool> InsereReceberPorLista(List<PagarReceber> pagarReceber);
        Task<bool> AtualizaReceberPorLista(List<PagarReceber> pagarReceber);
    }
}
