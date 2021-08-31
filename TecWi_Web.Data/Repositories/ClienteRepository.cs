using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        public Task<bool> BulkInsert(List<Cliente> cliente)
        {
            throw new NotImplementedException();
        }

        public Task<List<Cliente>> ListAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Cliente cliente)
        {
            throw new NotImplementedException();
        }
    }
}
