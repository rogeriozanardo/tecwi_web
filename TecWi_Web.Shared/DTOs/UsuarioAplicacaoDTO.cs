using System;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Shared.DTOs
{
    public class UsuarioAplicacaoDTO
    {
        public Guid IdUsuario { get; set; }
        public bool StAtivo { get; set; }
        public IdAplicacao IdAplicacao { get; set; }
        public string DsAplicacao { get; set; }
        public IdPerfil IdPerfil { get; set; }
        public bool StAtivo { get; set; }
    }
}
