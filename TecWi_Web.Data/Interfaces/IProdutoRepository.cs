using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Data.Interfaces
{
    public interface IProdutoRepository
    {
        Task SincronizarBase(List<Produto> produtos);

        Task<IEnumerable<ProdutoMercoCampDTO>> BuscarProdutosPorPeriodoSincronizacao(DateTime final);
    }
}
