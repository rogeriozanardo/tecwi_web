using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Dapper;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class PagarReceberRepository : IPagarReceberRepository
    {
        private readonly IDapper _iDapper;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _iConfiguration;
        public PagarReceberRepository(IDapper iDapper, DataContext dataContext, IConfiguration iConfiguration)
        {
            _iDapper = iDapper;
            _dataContext = dataContext;
            _iConfiguration = iConfiguration;
        }

        public async Task<bool> BulkInsertEfCore(List<PagarReceber> pagarReceber)
        {
            await _dataContext.Set<PagarReceber>().AddRangeAsync(pagarReceber);
            return true;
        }

        public bool BulkUpdateEfCore(List<PagarReceber> pagarReceber)
        {
            _dataContext.Set<PagarReceber>().UpdateRange(pagarReceber);
            return true;
        }

        public async Task<List<PagarReceber>> GetPenddingPagarReceber()
        {
            List<PagarReceber> pagarReceber = new List<PagarReceber>();
            DynamicParameters dynamicParameters = new DynamicParameters();

            string sql = GetPendingPagarReceber();
            List<string> connectionString = GetConnectionString();
            List<Task<List<PagarReceber>>> tasks = new List<Task<List<PagarReceber>>>();

            foreach (string _connectionString in connectionString)
            {
                tasks.Add(_iDapper.GetAll<PagarReceber>(sql, dynamicParameters, _connectionString));
            }

            await Task.WhenAll(tasks);

            foreach (Task<List<PagarReceber>> _task in tasks)
            {
                pagarReceber.AddRange(_task.Result);
            }

            return pagarReceber;
        }

        private string GetPendingPagarReceber()
        {
            return @"select top 5 p.SeqID,  p.numlancto, p.sq, p.cdfilial, p.dtemissao, p.dtvencto, p.valorr, string_agg(nf.NumNota, ', ') as NotasFiscais,  
            p.cdclifor, e.inscrifed,  e.fantasia, e.razao, e.ddd, e.fone1, e.fone2, e.email, trim(e.cidade)+ ' - ' + e.uf as cidade from 

            PagarReceber p with(nolock)

            left join ntfiscal nf with(nolock) on nf.NumPedido=p.numpedido
            inner join cliente c with(nolock) on c.cdcliente = p.cdclifor
            inner join empresa e with(nolock) on e.empresaid = c.empresaid

            where p.tppagrec='R'
            and p.stprevisao='R'
            and p.dtpagto is null
            and p.numtransf is null
            and p.tpdocquit = '4'
            and p.dtvencto < getdate()-2

            group by p.SeqID, p.numlancto, p.sq, p.cdfilial, p.dtemissao, p.dtvencto, p.valorr, p.cdclifor, e.inscrifed, e.fantasia, e.razao, e.ddd,
            e.fone1, e.fone2, e.email, e.cidade, e.uf";
        }

        private List<string> GetConnectionString()
        {
            return new List<string>()
            {
                _iConfiguration.GetConnectionString("DbTecwiFilial01"),
                _iConfiguration.GetConnectionString("DbTecwiFilial02"),
                _iConfiguration.GetConnectionString("DbTecWiMatriz")
            };
        }

        public async Task<List<PagarReceber>> GetAllEfCore()
        {
            List<PagarReceber> pagarReceber = await _dataContext.PagarReceber.ToListAsync();
            return pagarReceber;
        }
    }
}
