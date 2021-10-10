using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces
{
    public interface IUsuarioAplicacaoService
    {
        Task<ServiceResponse<Guid>> InsertAsync(UsuarioAplicacaoDTO usuarioAplicacaoDTO);
        Task<ServiceResponse<bool>> BulkInsertAsync(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO);
        Task<ServiceResponse<bool>> DeleteAsync(Guid idUsuario);
        Task<ServiceResponse<List<UsuarioAplicacaoDTO>>> GetByIdUsuarioAsync(Guid idUsuario);
        Task<ServiceResponse<bool>> UpdateAsync(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO);
    }
}
