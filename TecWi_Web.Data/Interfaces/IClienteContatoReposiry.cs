using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface IClienteContatoReposiry
    {
        Task<Guid> InsertAsync(ClienteContato clienteContato);
        Task<bool> UpdateAsync(ClienteContato clienteContato);
        Task<List<ClienteContato>> GetByClienteAsync(int Cdclifor);
        Task<bool> DeleteAsync(Guid IdClienteContato);
    }
}
