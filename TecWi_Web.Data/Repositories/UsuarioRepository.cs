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
            Usuario usuario = await _dataContext.Usuario.Include(x => x.UsuarioAplicacao).FirstOrDefaultAsync(x => x.Email == Email);
            return usuario;
        }

        public async Task<Usuario> GetByLoginAsync(string Login)
        {
            Usuario usuario = await _dataContext.Usuario.Include(x => x.UsuarioAplicacao).FirstOrDefaultAsync(x => x.Login == Login);
            return usuario;
        }

        public async Task<Usuario> GetByIdAsync(Guid Idusuario)
        {
            Usuario usuario = await _dataContext.Usuario.Include(x => x.UsuarioAplicacao).FirstOrDefaultAsync(x => x.IdUsuario == Idusuario);
            return usuario;
        }

        public async Task<List<Usuario>> GetAllAsync(UsuarioFilter usuarioFilter)
        {
            IQueryable<Usuario> _usuario = _dataContext.Usuario
                .Include(x => x.UsuarioAplicacao)
                .Where(x => !string.IsNullOrWhiteSpace(usuarioFilter.Nome) ? x.Nome == usuarioFilter.Nome : true)
                .Where(x => Enum.IsDefined(usuarioFilter.IdPerfil) ? x.UsuarioAplicacao.Any(x => x.IdPerfil == usuarioFilter.IdPerfil) : true)
                .Where(x => Enum.IsDefined(usuarioFilter.IdAplicacao) ? x.UsuarioAplicacao.Any(y => y.IdAplicacao == usuarioFilter.IdAplicacao) : true);

            List<Usuario> usuario = await _usuario
                .AsNoTracking()
                .ToListAsync();

            return usuario;
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
