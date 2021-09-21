using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<ServiceResponse<List<UsuarioDTO>>> GetAllAsycn(UsuarioFilter usuarioFilter);
        Task<ServiceResponse<bool>> Update(UsuarioDTO usuarioDTO);
    }
}
