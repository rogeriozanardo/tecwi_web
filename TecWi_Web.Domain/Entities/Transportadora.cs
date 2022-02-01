using System;

namespace TecWi_Web.Domain.Entities
{
    public class Transportadora
    {
        public int IdTransportadora { get; set; }
        public string CdTransportadora { get; set; }
        public string Inscrifed { get; set; }
        public string Tpinscricao { get; set; }
        public string Nome { get; set; }
        public string Fantasia { get; set; }
        public string Cidade { get; set; }
        public DateTime UpdRegistro { get; set; }
        public string StAtivo { get; set; }
        public string Email { get; set; }
    }
}
