using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces.JobsSincronizacao;
using TecWi_Web.Data.Dapper;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.Repositories.Sincronizacao.Interfaces;
using TecWi_Web.Data.Repositories.Sincronizacao.Repositorios;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Application.Services.JobsSincronizacao
{
    public class ProdutoSincronizacaoService : IProdutoSincronizacaoService
    {
        private readonly IProdutoRepository _ProdutoRepository;
        private readonly IProdutoSincronizacaoRepository _ProdutoSincronizacaoRepository;
        private readonly IParametroSincronizacaoProdutoRepository _ParametroSincronizacaoProdutoRepository;
        private readonly IDapper _Dapper;

        public ProdutoSincronizacaoService(IConfiguration configuration,
                                           IProdutoRepository produtoRepository,
                                           IParametroSincronizacaoProdutoRepository parametroSincronizacaoRepository,
                                           IDapper dapper)
        {
            _ProdutoRepository = produtoRepository;
            _Dapper = dapper;
            _ParametroSincronizacaoProdutoRepository = parametroSincronizacaoRepository;
            _ProdutoSincronizacaoRepository = new ProdutoSincronizacaoRepository(configuration.GetConnectionString("DbTecWiMatriz"), _Dapper);
        }

        public async Task Sincronizar()
        {
            DateTime ultimoUpdateDataSincronizacao = await _ParametroSincronizacaoProdutoRepository.BuscarUltimaDataUpdateSincronizacao();
            List<Produto> produtosMatriz = await _ProdutoSincronizacaoRepository.BuscarProdutosPorUltimaDataAlteracao(ultimoUpdateDataSincronizacao);

            if (produtosMatriz != null && produtosMatriz.Any())
            {
                await _ProdutoRepository.SincronizarBase(produtosMatriz);
                long ultimoRowVersion = produtosMatriz.Max(t => BitConverter.ToInt64(t.rowversion, 0));
                var rowVersionUltimo = BitConverter.GetBytes(ultimoRowVersion);
                await _ParametroSincronizacaoProdutoRepository.Inserir(new ParametroSincronizacaoProduto
                {
                    DataHoraSincronizacao = DateTime.Now,
                    RowVersion = rowVersionUltimo,
                    UltimoUpdateDate = produtosMatriz.Max(t => t.updregistro) ?? DateTime.MinValue,
                });
            }
        }
    }
}
