using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Interfaces
{
    public interface IContatoCobrancaRepository
    {
        Task<int> InsertAsync(ContatoCobranca contatoCobranca);

        Task<List<ContatoCobranca>> GetAllAsync(ContatoCobrancaFilter contatoCobrancaFilter);
    }
}
