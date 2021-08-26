using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces
{
    public interface IAutorizacaoService
    {
        Task<ServiceResponse<UsuarioDTO>> Registrar(UsuarioDTO usuarioDTO);

        Task<ServiceResponse<string>> Login(UsuarioDTO usuarioDTO);
    }
}
