using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface IPagarReceberRepository
    {
        Task<List<PagarReceber>> GetPenddingPagarReceber();
        Task<List<PagarReceber>> GetAllEfCore();
        bool BulkUpdateEfCore(List<PagarReceber> pagarReceber);
        Task<bool> BulkInsertEfCore(List<PagarReceber> pagarReceber);
    }
}
