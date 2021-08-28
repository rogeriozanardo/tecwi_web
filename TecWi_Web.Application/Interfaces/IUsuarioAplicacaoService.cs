using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces
{
    public interface IUsuarioAplicacaoService
    {
        Task<ServiceResponse<Guid>> Insert(UsuarioAplicacaoDTO usuarioAplicacaoDTO);
        Task<ServiceResponse<bool>> BulkInsert(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO);
        Task<ServiceResponse<bool>> Delete(Guid idUsuario);
        Task<ServiceResponse<List<UsuarioAplicacaoDTO>>> GetByIdUsuario(Guid idUsuario);
    }
}
