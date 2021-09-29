using System;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public class UsuarioAplicacao
    {
        public Usuario Usuario { get; set; }
        public Guid IdUsuario { get; set; }
        public bool StAtivo { get; set; }
        public IdAplicacao IdAplicacao { get; set; }
        public IdPerfil IdPerfil { get; set; }

        public UsuarioAplicacao(Guid idUsuario, bool stAtivo, IdAplicacao idAplicacao, IdPerfil idPerfil)
        {
            ValidateDomain(idUsuario, stAtivo, idAplicacao, idPerfil);
        }

        public UsuarioAplicacao()
        {

        }

        private string IdAplicacaoInvalido = "Id aplicação inválido!";
        private string IdPerfilInvalido = "Id perfil inválido!";
        private void ValidateDomain(Guid idUsuario, bool stAtivo, IdAplicacao idAplicacao, IdPerfil idPerfil)
        {
            IdUsuario = idUsuario;

            StAtivo = stAtivo;

            DomainValidadorException.Whem((int)idAplicacao == 0, IdAplicacaoInvalido);
            IdAplicacao = idAplicacao;

            DomainValidadorException.Whem((int)idPerfil == 0, IdPerfilInvalido);
            IdPerfil = idPerfil;
        }
    }
}
