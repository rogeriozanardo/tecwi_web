using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Interfaces
{
    public interface IAutorizacaoService
    {
        Task<ServiceResponse<UsuarioDTO>> LoginAsync(UsuarioDTO usuarioDTO);
    }
}
