using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> ListAll();
        Task<bool> BulkInsert(List<Cliente> cliente);
        Task<bool> Update(Cliente cliente);
    }
}
