using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;


namespace TecWi_Web.Application.Interfaces
{
    public interface IContatoCobrancaService
    {
        Task<ServiceResponse<int>> InsertAsync(ContatoCobrancaDTO contatoCobrancaDTO);

        Task<ServiceResponse<List<ContatoCobrancaDTO>>> GetAllAsync(ContatoCobrancaFilter contatoCobrancaFilter);
    }
}
