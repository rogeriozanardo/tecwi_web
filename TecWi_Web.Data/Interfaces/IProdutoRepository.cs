using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface IProdutoRepository
    {
        Task SincronizarBase(List<Produto> produtos);
    }
}
