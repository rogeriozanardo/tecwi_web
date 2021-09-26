using System;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Shared.Filters
{
    public class ContatoCobrancaFilter : FilterBase
    {
        public int IdCliente { get; set; }
        public Guid IdUsuario { get; set; }
        public DateTime? DtContatoStart { get; set; }
        public DateTime? DtContatoEnd { get; set; }
        public string Anotacao { get; set; }
        public TipoContatoEnum TipoContato { get; set; }
        public DateTime? DtAgendaStart { get; set; }
        public DateTime? DtAgendaEnd { get; set; }
    }
}
