using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Shared.DTOs
{
    public class AtendimentoDetalhesDTO
    {
        public Guid IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public int Cdclifor { get; set; }
        public string Razao { get; set; }
        public Guid IdContato { get; set; }
        public DateTime DtContato { get; set; }
        public TipoContatoEnum TipoContato { get; set; }
        public string Anotacao { get; set; }
    }
}
