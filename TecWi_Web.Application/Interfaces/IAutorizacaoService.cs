using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Interfaces
{
    public interface IAutorizacaoService
    {
        Task<ServiceResponse<UsuarioDTO>> Register(UsuarioDTO usuarioDTO);

        Task<ServiceResponse<UsuarioDTO>> Login(UsuarioDTO usuarioDTO);

        Task<ServiceResponse<List<UsuarioDTO>>> GetUserList(UsuarioFilter usuarioFilter);
    }
}
