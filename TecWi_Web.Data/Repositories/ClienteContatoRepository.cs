using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class ClienteContatoRepository : IClienteContatoReposiry
    {
        private readonly DataContext _dataContext;
        public ClienteContatoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DeleteAsync(Guid IdClienteContato)
        {
            ClienteContato clienteContato = await _dataContext.Set<ClienteContato>().FirstOrDefaultAsync(x => x.IdClienteContato == IdClienteContato);
            if (clienteContato != null)
            {
                _dataContext.Set<ClienteContato>().Remove(clienteContato);
            }

            return true;
        }

        public async Task<List<ClienteContato>> GetByClienteAsync(int Cdclifor)
        {
            IQueryable<ClienteContato> _clienteContato = _dataContext.ClienteContato
                .Where(x => x.Cdclifor == Cdclifor);

            List<ClienteContato> clienteContato = await _clienteContato
                .AsNoTracking()
                .ToListAsync();

            return clienteContato;
        }

        public async Task<Guid> InsertAsync(ClienteContato clienteContato)
        {
            await _dataContext.Set<ClienteContato>().AddAsync(clienteContato);

            return clienteContato.IdClienteContato;
        }

        public async Task<bool> UpdateAsync(ClienteContato clienteContato)
        {

            await Task.Run(() =>
           {
               _dataContext.Set<ClienteContato>().Update(clienteContato);
           });

            return true;
        }
    }
}