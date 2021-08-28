using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;

namespace TecWi_Web.Data.Interfaces
{
    public interface IUsuarioAplicacaoRepository
    {
        Task<Guid> Insert(UsuarioAplicacao usuarioAplicacao);
        Task<bool> BulkInsert(List<UsuarioAplicacao> usuarioAplicacao);
        Task<bool> Delete(Guid idUsuario);
        Task<List<UsuarioAplicacao>> GetByIdUsuario(Guid idUsuario);
    }
}
