using System.Collections.Generic;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public  class ContatoCobrancaLancamento
    {
        public ContatoCobrancaLancamento(int IdContato)
        {
            ValidateDomain(IdContato);
        }

        public int IdContato { get; set; }
        public string Numlancto { get; private set; }
        public int Sq { get; set; }
        public string Cdfilial { get; private set; }
        public ContatoCobranca ContatoCobranca { get; set; }
        public PagarReceber PagarReceber { get; set; }

        private string IdContatoInvalido = "Campo 'IdContato' inválido!";
        private void ValidateDomain(int idContato)
        {
            DomainValidadorException.Whem(idContato == 0, IdContatoInvalido);
            IdContato = idContato;
        }
    }
}
