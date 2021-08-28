using System;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Guid> Insert(Usuario usuario);
        void Update(Usuario usuario);
        Task<Usuario> GetByEmailAsync(string Email);
    }
}
