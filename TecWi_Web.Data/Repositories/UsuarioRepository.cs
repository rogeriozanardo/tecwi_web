using System;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using Microsoft.EntityFrameworkCore;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DataContext _dataContext;
        public UsuarioRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Usuario> GetByEmailAsync(string Email)
        {
            Usuario usuario = await _dataContext.Usuario.FirstOrDefaultAsync(x => x.Email == Email);
            return usuario;
        }

        public Guid Insert(Usuario usuario)
        {
            _dataContext.Set<Usuario>().Add(usuario);
            return usuario.IdUsuario;
        }

        public void Update(Usuario usuario)
        {
            _dataContext.Set<Usuario>().Update(usuario);
        }
    }
}
