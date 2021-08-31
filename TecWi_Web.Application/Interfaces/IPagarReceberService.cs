using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces
{
    public interface IPagarReceberService
    {
        Task<ServiceResponse<bool>> BulkInsertEfCore(List<PagarReceberDTO> pagarReceberDTO);
        Task<ServiceResponse<bool>> BulkUpdateEfCore(List<PagarReceberDTO> pagarReceberDTO);
        ServiceResponse<List<PagarReceberDTO>> GetAllDapper();
        Task<ServiceResponse<List<PagarReceberDTO>>> GetAllEfCore();
    }
}
