using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces
{
    public interface IPagarReceberService
    {
        Task<ServiceResponse<bool>> PopulateData();
    }
}
