using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetAllAsync(ClientePagarReceberFilter clientePagarReceberFilter);
        Task<bool> BulkInsertAsync(List<Cliente> cliente);
        bool BulkUpdate(List<Cliente> cliente);
    }
}
