using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly DataContext _dataContext;

        public ClienteRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> BulkInsertOrUpdateAsync(List<Cliente> cliente)
        {
            await _dataContext.AddRangeAsync(cliente);
            return true;
        }

        public async Task<List<Cliente>> GetAllAsync(ClientePagarReceberFilter clientePagarReceberFilter)
        {
            IQueryable<Cliente> _cliente = _dataContext.Cliente
                .Include(x => x.PagarReceber)
                .Where(x => clientePagarReceberFilter.cdclifor != null ? x.Cdclifor == clientePagarReceberFilter.cdclifor : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.inscrifed) ? x.Inscrifed.Contains(clientePagarReceberFilter.inscrifed) : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.fantasia) ? x.Fantasia.Contains(clientePagarReceberFilter.fantasia) : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.razao) ? x.Fantasia.Contains(clientePagarReceberFilter.razao) : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.numlancto) ? x.PagarReceber.Where(y => y.Numlancto.Contains(clientePagarReceberFilter.numlancto)).ToList().Count > 0 : true)
                .Where(x => clientePagarReceberFilter.dtemissaoStart != null ? x.PagarReceber.Where(y => y.Dtemissao >= (DateTime)clientePagarReceberFilter.dtemissaoStart).ToList().Count > 0 : true)
                .Where(x => clientePagarReceberFilter.dtemissaoEnd != null ? x.PagarReceber.Where(y => y.Dtemissao <= (DateTime)clientePagarReceberFilter.dtemissaoEnd).ToList().Count > 0 : true)
                .Where(x => clientePagarReceberFilter.dtvenctoStart != null ? x.PagarReceber.Where(y => y.Dtvencto >= (DateTime)clientePagarReceberFilter.dtvenctoStart).ToList().Count > 0 : true)
                .Where(x => clientePagarReceberFilter.dtvenctoEnd != null ? x.PagarReceber.Where(y => y.Dtvencto <= (DateTime)clientePagarReceberFilter.dtvenctoEnd).ToList().Count > 0 : true)
                .Where(x => !string.IsNullOrWhiteSpace(clientePagarReceberFilter.NotasFiscais) ? x.PagarReceber.Where(y => y.NotasFiscais.Contains(clientePagarReceberFilter.NotasFiscais)).ToList().Count > 0 : true);

                List<Cliente> cliente = await _cliente
                .AsNoTracking()
                .Skip((clientePagarReceberFilter.PageNumber - 1) * clientePagarReceberFilter.PageSize)
                .Take(clientePagarReceberFilter.PageSize)
                .ToListAsync();

            return cliente;
        }
    }
}
