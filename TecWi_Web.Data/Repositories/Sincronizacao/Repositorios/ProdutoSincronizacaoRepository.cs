using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TecWi_Web.Data.Dapper;
using TecWi_Web.Data.Repositories.Querys;
using TecWi_Web.Data.Repositories.Sincronizacao.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories.Sincronizacao.Repositorios
{
    public class ProdutoSincronizacaoRepository : IProdutoSincronizacaoRepository
    {
        private readonly string _ConnectionString;
        private readonly IDapper _Dapper;

        public ProdutoSincronizacaoRepository(string connectionString, 
                                              IDapper dapper)
        {
            _ConnectionString = connectionString;
            _Dapper = dapper;
        }

        public string connectionString => _ConnectionString;

        public async Task<List<Produto>> BuscarProdutosPorUltimaDataAlteracao(DateTime ultimaAlteracao)
        {
            string sql = ProdutoQuery.QUERY_SELECT_PRODUTOS;
            DynamicParameters dynamicParameters = PopularParametrosDataUltimaAlteracao(ultimaAlteracao);
            return await _Dapper.GetAll<Produto>(sql, dynamicParameters, connectionString); 
        }

        private DynamicParameters PopularParametrosDataUltimaAlteracao(DateTime ultimaAlteracao)
        {
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("@UPDATE_DATE", ultimaAlteracao, DbType.DateTime);
            parametros.Add("@DATA_ATUAL", DateTime.Now, DbType.DateTime);

            return parametros;
        }
    }
}
