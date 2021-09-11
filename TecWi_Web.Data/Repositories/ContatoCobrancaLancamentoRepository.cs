using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Repositories
{
    public class ContatoCobrancaLancamentoRepository : IContatoCobrancaLancamentoRepository
    {
        private readonly DataContext _dataContext;
        public ContatoCobrancaLancamentoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ContatoCobrancaLancamento>> GetAllAsyc(ContatoCobrancaLancamentoFilter contatoCobrancaLancamentoFilter)
        {
            IQueryable<ContatoCobrancaLancamento> _contatoCobrancaLancamento = _dataContext.ContatoCobrancaLancamento
                .Where(x => contatoCobrancaLancamentoFilter.IdContato != 0 ? x.IdContato == contatoCobrancaLancamentoFilter.IdContato : true)
                .Where(x => !string.IsNullOrWhiteSpace(contatoCobrancaLancamentoFilter.Numlancto) ? x.Numlancto == contatoCobrancaLancamentoFilter.Numlancto : true)
                .Where(x => contatoCobrancaLancamentoFilter.Sq != 0 ? x.Sq == contatoCobrancaLancamentoFilter.Sq : true)
                .Where(x => !string.IsNullOrWhiteSpace(contatoCobrancaLancamentoFilter.Cdfilial) ? x.CdFilial == contatoCobrancaLancamentoFilter.Cdfilial : true);

            List<ContatoCobrancaLancamento> contatoCobrancaLancamento = await _contatoCobrancaLancamento
                .AsNoTracking()
                .ToListAsync();

            return contatoCobrancaLancamento;
        }

        public async Task BulkInsertAsync(List<ContatoCobrancaLancamento> contatoCobrancaLancamento)
        {
            await _dataContext.ContatoCobrancaLancamento.AddRangeAsync(contatoCobrancaLancamento);
        }
    }
}