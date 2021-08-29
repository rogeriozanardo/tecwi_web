using System;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public class UsuarioAplicacao
    {
        public Usuario Usuario { get; set; }
        public Guid IdUsuario { get; private set; }
        public IdAplicacao IdAplicacao { get; private set; }
        public IdPerfil IdPerfil { get; private set; }

        public UsuarioAplicacao(Guid idUsuario, IdAplicacao idAplicacao, IdPerfil idPerfil)
        {
            ValidateDomain(idUsuario, idAplicacao, idPerfil);
        }

        private string IdAplicacaoInvalido = "Id aplicação inválido!";
        private string IdPerfilInvalido = "Id perfil inválido!";
        private void ValidateDomain(Guid idUsuario, IdAplicacao idAplicacao, IdPerfil idPerfil)
        {
            IdUsuario = idUsuario;

            DomainValidadorException.Whem((int)idAplicacao == 0, IdAplicacaoInvalido);
            IdAplicacao = idAplicacao;

            DomainValidadorException.Whem((int)idPerfil == 0, IdPerfilInvalido);
            IdPerfil = idPerfil;
        }
    }
}
