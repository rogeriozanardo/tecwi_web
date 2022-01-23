using System;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface IParametroSincronizacaoProdutoRepository
    {
        Task<DateTime> BuscarUltimaDataUpdateSincronizacao();

        Task Inserir(ParametroSincronizacaoProduto parametroSincronizacaoProduto);
    }
}
