using System;
using System.Collections.Generic;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public class ContatoCobranca
    {
        public ContatoCobranca(int idCliente, Guid IdUsuario, DateTime dtContato, string anotacao, TipoContato tipoContato, DateTime dtAgenda)
        {
            ValidateDomain(idCliente, IdUsuario, dtContato, anotacao, tipoContato, dtAgenda);
        }

        public int IdContato { get; private set; }
        public int IdCliente { get; private set; }
        public Guid IdUsuario { get; private set; }
        public DateTime DtContato { get; private set; }
        public string Anotacao { get; private set; }
        public TipoContato TipoContato { get; private set; }
        public DateTime DtAgenda { get; private set; }
        public List<ContatoCobrancaLancamento> ContatoCobrancaLancamento { get; set; }

        private string IdClienteInvalido = "Cliente inválido!";
        private string IdUsuarioInvalido = "Usuário inválido!";
        private string DtContatoInvalido = "Data contato inválida!";
        private string AnotacaoInvalido = "Anotação inválida!";
        private string DataAgendaInvalido = "Data agenda inválida!";
        private void ValidateDomain(int idCliente, Guid idUsuario, DateTime dtContato, string anotacao, TipoContato tipoContato, DateTime dtAgenda)
        {
            DomainValidadorException.Whem(IdCliente == 0, IdClienteInvalido);
            IdCliente = idCliente;

            DomainValidadorException.Whem(idUsuario == new Guid(), IdUsuarioInvalido);
            IdUsuario = idUsuario;

            DomainValidadorException.Whem(dtContato.DayOfYear != DateTime.Now.DayOfYear, DtContatoInvalido);
            DtContato = dtContato;

            DomainValidadorException.Whem(string.IsNullOrWhiteSpace(anotacao), AnotacaoInvalido);
            Anotacao = anotacao;

            TipoContato = tipoContato;

            DomainValidadorException.Whem(dtAgenda.DayOfYear != DateTime.Now.DayOfYear, DataAgendaInvalido);
            DtAgenda = dtAgenda;
        }
    }
}