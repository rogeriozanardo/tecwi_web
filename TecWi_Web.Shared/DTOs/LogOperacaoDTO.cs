using System;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Shared.DTOs
{
    public class LogOperacaoDTO
    {
        public Guid IdLogOperacao { get; private set; }
        public Guid IdUsuario { get; private set; }
        public TipoOperacao TipoOperacao { get; private set; }
        public DateTime Data { get; private set; } = DateTime.Now;
        public UsuarioDTO UsuarioDTO { get; set; }
    }
}
