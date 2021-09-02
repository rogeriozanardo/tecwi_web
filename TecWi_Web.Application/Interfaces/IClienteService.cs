using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ServiceResponse<bool>> BulkInsertAsync(List<ClienteDTO> clienteDTO);
        Task<ServiceResponse<bool>> BulkUpdateAsync(List<ClienteDTO> clienteDTO);
        Task<ServiceResponse<List<ClienteDTO>>> GetAllAsync(ClientePagarReceberFilter clientePagarReceberFilter);
    }
}
