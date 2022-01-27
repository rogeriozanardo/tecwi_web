using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DataContext _dataContext;
        const int TIME_OUT_BULK_COPY = 3000;
        const int TIME_OUT_ENTITY = 4;
        const int TEMPO_SINCRONIZACAO_CRON_JOB_ENVIO_PRODUTO = 20;

        public ProdutoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<ProdutoMercoCampDTO>> BuscarProdutosPorPeriodoSincronizacao(DateTime final)
        {
            //Removi 20 minutos do último minuto pesquisado para garantir que não vai ter gaps entre as concorrencias dos processos.
            //Pode ocorrer do processo de envio ocorrer e o de sincronia de bases também. Aí pode acontecer de produtos não terem sido enviados.
            var produtos = await _dataContext.Produto.Where(t => t.updregistro >= final.AddMinutes(-TEMPO_SINCRONIZACAO_CRON_JOB_ENVIO_PRODUTO) && t.updregistro <= DateTime.Now)
                                                    .Select(t => new ProdutoMercoCampDTO
                                                    {
                                                        CdProduto = t.CdProduto,
                                                        DsVenda = t.DsVenda,
                                                        CodigoFornecedor = t.CdFornecPadrao ?? string.Empty,
                                                        NomeFornecedor = t.Marca ?? string.Empty,
                                                        Embalagens = new List<EmbalagemMercoCampDTO>()                 
                                                        {
                                                             new EmbalagemMercoCampDTO
                                                             {
                                                                UnidadeVenda = t.UnidadeVda,
                                                                CodigoBarra = t.CdBarra
                                                            }
                                                        }
                                                    })
                                                    .ToListAsync();

            return produtos;
        }

        public async Task SincronizarBase(List<Produto> produtos)
        {
           await InserirAlterarPorLote(produtos);
        }

        private async Task InserirAlterarPorLote(List<Produto> produtos)
        {
           var bulkconfig = new BulkConfig 
           { 
               BulkCopyTimeout = TIME_OUT_BULK_COPY,
               UseTempDB = false
           };
            _dataContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(TIME_OUT_ENTITY));
            _dataContext.BulkInsertOrUpdate(produtos, bulkconfig);
            await _dataContext.SaveChangesAsync();
        } 
    }
}
