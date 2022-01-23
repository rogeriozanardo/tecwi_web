using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly DataContext _dataContext;
        const int TAMANHO_MAXIMO_POR_LOTE = 1000;

        public ProdutoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task SincronizarBase(List<Produto> produtos)
        {
            //int totalRegistros = produtos.Count;
            //if(totalRegistros > TAMANHO_MAXIMO_POR_LOTE)
            //{
            //    int quantidadePorLote = TAMANHO_MAXIMO_POR_LOTE;
            //    int maximoIteracoes = (int)(totalRegistros / TAMANHO_MAXIMO_POR_LOTE);

            //    for (int iteracao = 0; iteracao < maximoIteracoes; iteracao++)
            //    {
            //        int inicio = quantidadePorLote * iteracao;
            //        var produtosPorLote = produtos.Skip(inicio).Take(quantidadePorLote).ToList();
            //        await InserirAlterarPorLote(produtosPorLote);
            //    }
            //}
            //else
                await InserirAlterarPorLote(produtos);
        }

        private async Task InserirAlterarPorLote(List<Produto> produtos)
        {
           var bulkconfig = new BulkConfig 
           { 
               BulkCopyTimeout = 3000,
               UseTempDB = false
           };
            _dataContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(4));
            _dataContext.BulkInsertOrUpdate(produtos, bulkconfig);
            await _dataContext.SaveChangesAsync();
        } 
    }
}
