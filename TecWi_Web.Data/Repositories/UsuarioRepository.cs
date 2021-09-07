using System;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using Microsoft.EntityFrameworkCore;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;
using System.Collections.Generic;
using TecWi_Web.Shared.Filters;
using System.Linq;

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

        public async Task<List<Usuario>> GetAllAsync(UsuarioFilter usuarioFilter)
        {
            IQueryable<Usuario> usuario = _dataContext.Usuario
                .Where(x => !string.IsNullOrWhiteSpace(usuarioFilter.None) ? x.Nome == usuarioFilter.None : true);

            List<Usuario> _usuario = await usuario
                .AsNoTracking()
                .ToListAsync();

            return _usuario;
        }

        public async Task<Guid> Insert(Usuario usuario)
        {
            await _dataContext.Set<Usuario>().AddAsync(usuario);
            return usuario.IdUsuario;
        }

        public void Update(Usuario usuario)
        {
            _dataContext.Set<Usuario>().Update(usuario);
        }
    }
}
