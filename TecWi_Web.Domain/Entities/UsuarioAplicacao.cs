using System;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Domain.Entities
{
    public class UsuarioAplicacao
    {
        public Usuario Usuario { get;set; }
        public Guid IdUsuario { get; set; }
        public IdAplicacao IdAplicacao { get; set; }
        public IdPerfil IdPerfil { get; set; }
    }
}
