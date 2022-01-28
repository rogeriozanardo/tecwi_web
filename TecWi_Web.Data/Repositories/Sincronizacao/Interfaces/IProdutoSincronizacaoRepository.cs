using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories.Sincronizacao.Interfaces
{
    public interface IProdutoSincronizacaoRepository
    {
        Task<List<Produto>> BuscarProdutosPorUltimaDataAlteracao(DateTime ultimaAlteracao);
        string connectionString { get; }
    }
}
