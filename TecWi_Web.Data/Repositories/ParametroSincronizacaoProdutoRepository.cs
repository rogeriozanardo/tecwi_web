using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Repositories.Sincronizacao.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class ParametroSincronizacaoProdutoRepository : IParametroSincronizacaoProdutoRepository
    {
        private readonly DataContext _DataContext;

        public ParametroSincronizacaoProdutoRepository(DataContext dataContext)
        {
            _DataContext = dataContext;
        }

        public async Task<DateTime> BuscarUltimaDataUpdateSincronizacao() 
        {
           var parametroSincronizacaoProduto = await _DataContext.ParametroSincronizacaoProduto
                                                                  .OrderByDescending(t => t.ID)
                                                                  .FirstOrDefaultAsync();

            //Data minima que o sql server aceita
            return parametroSincronizacaoProduto?.UltimoUpdateDate ?? new DateTime(1753, 01, 01);
        }

        public async Task Inserir(ParametroSincronizacaoProduto parametroSincronizacaoProduto)
        {
           await _DataContext.ParametroSincronizacaoProduto.AddAsync(parametroSincronizacaoProduto);
           await _DataContext.SaveChangesAsync();
        }
    }
}
