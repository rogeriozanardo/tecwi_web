using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> GetByEmailAsync(string Email);
        Task<Usuario> GetByLoginAsync(string Login);
        Task<Usuario> GetByIdAsync(Guid Idusuario);
        Task<List<Usuario>> GetAllAsync(UsuarioFilter usuarioFilter);
        Task<Guid> Insert(Usuario usuario);
        void Update(Usuario usuario);

    }
}
