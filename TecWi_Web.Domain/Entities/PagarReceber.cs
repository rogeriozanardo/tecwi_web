using System;

namespace TecWi_Web.Domain.Entities
{
    public class PagarReceber
    {
        public PagarReceber()
        {

        }

        public Cliente Cliente { get; set; }
        public int SeqID { get; private set; }
        public string Numlancto { get; private set; }
        public int Sq { get; set; }
        public bool Stcobranca { get; set; } = true;
        public string Cdfilial { get; private set; }
        public DateTime Dtemissao { get; private set; }
        public DateTime Dtvencto { get; private set; }
        public DateTime? Dtpagto { get; private set; }
        public decimal Valorr { get; private set; }
        public string NotasFiscais { get; private set; }
        public int Cdclifor { get; private set; }
        public string Inscrifed { get; private set; }
        public string Fantasia { get; private set; }
        public string Razao { get; private set; }
        public string DDD { get; private set; }
        public string Fone1 { get; private set; }
        public string Fone2 { get; private set; }
        public string Email { get; private set; }
        public string Cidade { get; private set; }

        public void Update(bool stcobranca, DateTime dtpagto)
        {
            Stcobranca = stcobranca;
            Dtpagto = dtpagto;
        }
    }
}
