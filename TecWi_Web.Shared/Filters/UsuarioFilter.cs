using System;

namespace TecWi_Web.Shared.Filters
{
    public class UsuarioFilter : FilterBase
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public Guid IdUsuario { get; set; }
    }
}
