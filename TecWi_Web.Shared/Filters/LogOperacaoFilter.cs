using System;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Shared.Filters
{
    public class LogOperacaoFilter : FilterBase
    {
        public Guid IdUsuario { get; set; }
        public TipoOperacao tipoOperacao { get; set; }
        public DateTime? DataStart { get; set; }
        public DateTime? DataEnd { get; set; }
    }
}
