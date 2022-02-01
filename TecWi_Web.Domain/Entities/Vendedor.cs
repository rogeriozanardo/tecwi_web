using System;

namespace TecWi_Web.Domain.Entities
{
    public class Vendedor
    {
        public int IdVendedor { get; set; }
        public string CdVendedor { get; set; }
        public string Apelido { get; set; }
        public string Nome { get; set; }
        public DateTime? UpdRegistro { get; set; }
    }
}
