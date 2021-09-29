using System;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Shared.Filters
{
    public class UsuarioFilter : FilterBase
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public Guid IdUsuario { get; set; }
        public IdAplicacao IdAplicacao { get; set; }
        public IdPerfil IdPerfil { get; set; }
    }
}
