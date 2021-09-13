using System.Collections.Generic;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public  class ContatoCobrancaLancamento
    {
        public ContatoCobrancaLancamento(int idContato, string numlancto, int sq, string cdfilial)
        {
            ValidateDomain(idContato, numlancto, sq, cdfilial);
        }

        public ContatoCobrancaLancamento()
        {

        }

        public int IdContato { get; set; }
        public string Numlancto { get; private set; }
        public int Sq { get; private set; }
        public string CdFilial { get; private set; }
        public ContatoCobranca ContatoCobranca { get; set; }
        public PagarReceber PagarReceber { get; set; }

        private string IdContatoInvalido = "Campo 'IdContato' inválido!";
        private string NumlanctoInvalido = "Campo 'Numlancto' inválido!";
        private string SqInvalido = "Campo 'Sq' inválido!";
        private string CdfilialInvalido = "Campo 'Cdfilial' inválido!";
        private void ValidateDomain(int idContato, string numlancto, int sq, string cdfilial)
        {
            DomainValidadorException.Whem(idContato == 0, IdContatoInvalido);
            IdContato = idContato;

            DomainValidadorException.Whem(string.IsNullOrWhiteSpace(numlancto), NumlanctoInvalido);
            Numlancto = numlancto;

            DomainValidadorException.Whem(sq == 0, SqInvalido);
            Sq = sq;

            DomainValidadorException.Whem(string.IsNullOrWhiteSpace(cdfilial), CdfilialInvalido);
            CdFilial = cdfilial;
        }
    }
}
