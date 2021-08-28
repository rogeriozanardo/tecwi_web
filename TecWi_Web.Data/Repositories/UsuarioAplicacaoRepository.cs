using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Repositories
{
    public class UsuarioAplicacaoRepository : IUsuarioAplicacaoRepository
    {
        private readonly DataContext _dataContext;
        public UsuarioAplicacaoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> BulkInsert(List<UsuarioAplicacao> usuarioAplicacao)
        {
            await _dataContext.Set<UsuarioAplicacao>().AddRangeAsync(usuarioAplicacao);
            return true;
        }

        public async Task<bool> Delete(Guid idUsuario)
        {
            List<UsuarioAplicacao> uuarioAplicacao = await _dataContext.Set<UsuarioAplicacao>().Where(x => x.IdUsuario == idUsuario).ToListAsync();
            _dataContext.Set<UsuarioAplicacao>().RemoveRange(uuarioAplicacao);
            return true;
        }

        public async Task<List<UsuarioAplicacao>> GetByIdUsuario(Guid idUsuario)
        {
            List<UsuarioAplicacao> usuarioAplicacao = await _dataContext.Set<UsuarioAplicacao>().Where(x => x.IdUsuario == idUsuario).ToListAsync();
            return usuarioAplicacao;
        }

        public async Task<Guid> Insert(UsuarioAplicacao usuarioAplicacao)
        {
            await _dataContext.Set<UsuarioAplicacao>().AddAsync(usuarioAplicacao);
            return usuarioAplicacao.IdUsuario;
        }
    }
}
