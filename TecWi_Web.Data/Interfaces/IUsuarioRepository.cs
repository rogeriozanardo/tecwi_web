using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Guid> Insert(Usuario usuario);
        void Update(Usuario usuario);
        Task<Usuario> GetByEmailAsync(string Email);

        Task<List<Usuario>> GetAllAsync(UsuarioFilter usuarioFilter);
    }
}
