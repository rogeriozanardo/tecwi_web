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
    public class ContatoCobrancaRepository : IContatoCobrancaRepository
    {
        private readonly DataContext _dataContext;
        public ContatoCobrancaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<ContatoCobranca>> GetAllAsync(ContatoCobrancaFilter contatoCobrancaFilter)
        {
            IQueryable<ContatoCobranca> _contatoCobranca = _dataContext.ContatoCobranca
                .Include(x => x.ContatoCobrancaLancamento)
                .Where(x => contatoCobrancaFilter.IdCliente != 0 ? x.IdCliente == contatoCobrancaFilter.IdCliente : true)
                .Where(x => contatoCobrancaFilter.IdUsuario != new Guid() ? x.IdUsuario == contatoCobrancaFilter.IdUsuario : true)
                .Where(x => contatoCobrancaFilter.DtContatoStart != null ? x.DtContato >= contatoCobrancaFilter.DtContatoStart : true)
                .Where(x => contatoCobrancaFilter.DtContatoEnd != null ? x.DtContato <= contatoCobrancaFilter.DtContatoEnd : true)
                .Where(x => !string.IsNullOrWhiteSpace(contatoCobrancaFilter.Anotacao) ? x.Anotacao.Contains(contatoCobrancaFilter.Anotacao) : true)
                .Where(x => contatoCobrancaFilter.DtAgendaStart != null ? x.DtAgenda >= contatoCobrancaFilter.DtAgendaStart : true)
                .Where(x => contatoCobrancaFilter.DtAgendaEnd != null ? x.DtAgenda <= contatoCobrancaFilter.DtAgendaEnd : true);

            List<ContatoCobranca> contatoCobranca = await _contatoCobranca
                .AsNoTracking()
                .ToListAsync();

            return contatoCobranca;
        }

        public async Task<int> InsertAsync(ContatoCobranca contatoCobranca)
        {
            await _dataContext.ContatoCobranca.AddAsync(contatoCobranca);
            return contatoCobranca.IdContato;
            
        }
    }
}