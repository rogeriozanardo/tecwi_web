using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<ServiceResponse<List<UsuarioDTO>>> GetAllAsync(UsuarioFilter usuarioFilter);
        Task<ServiceResponse<bool>> UpdateAsync(UsuarioDTO usuarioDTO);
        Task<ServiceResponse<Guid>> InsertAsync(UsuarioDTO UsuarioDTO);
        Task<ServiceResponse<UsuarioDTO>> GetByIdAsync(Guid Idusuario);
        Task<ServiceResponse<UsuarioDTO>> GetByLoginAsync(string Login);
    }
}
