using System;
using System.Collections.Generic;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public class ContatoCobranca
    {
        public ContatoCobranca(int Cdclifor, Guid IdUsuario, DateTime dtContato, string anotacao, TipoContatoEnum tipoContato, DateTime dtAgenda)
        {
            ValidateDomain(Cdclifor, IdUsuario, dtContato, anotacao, tipoContato, dtAgenda);
        }

        public ContatoCobranca()
        {

        }

        public Guid IdContato { get; private set; }
        public int Cdclifor { get; private set; }
        public Guid IdUsuario { get; private set; }
        public DateTime DtContato { get; private set; }
        public string Anotacao { get; private set; }
        public TipoContatoEnum TipoContato { get; private set; }
        public DateTime DtAgenda { get; private set; }
        public Usuario Usuario { get; set; }
        public Cliente Cliente { get; set; }
        public List<ContatoCobrancaLancamento> ContatoCobrancaLancamento { get; set; }

        private string IdContatoInvalido = "Id inválido!";
        private string CdcliforInvalido = "Cliente inválido!";
        private string IdUsuarioInvalido = "Usuário inválido!";
        private string DtContatoInvalido = "Data contato inválida!";
        private string AnotacaoInvalido = "Anotação inválida!";
        private string DataAgendaInvalido = "Data agenda inválida!";
        private void ValidateDomain(int cdclifor, Guid idUsuario, DateTime dtContato, string anotacao, TipoContatoEnum tipoContato, DateTime dtAgenda)
        {
            DomainValidadorException.Whem(cdclifor == 0, CdcliforInvalido);
            Cdclifor = cdclifor;

            DomainValidadorException.Whem(idUsuario == new Guid(), IdUsuarioInvalido);
            IdUsuario = idUsuario;

            DomainValidadorException.Whem(dtContato.DayOfYear != DateTime.Now.DayOfYear, DtContatoInvalido);
            DtContato = dtContato;

            DomainValidadorException.Whem(string.IsNullOrWhiteSpace(anotacao), AnotacaoInvalido);
            Anotacao = anotacao;

            TipoContato = tipoContato;

            DomainValidadorException.Whem(dtAgenda.DayOfYear < DateTime.Now.DayOfYear, DataAgendaInvalido);
            DtAgenda = dtAgenda;
        }

        public void Update(Guid idContato)
        {
            DomainValidadorException.Whem(idContato == Guid.Empty, IdContatoInvalido);
            IdContato = idContato;
        }
    }
}