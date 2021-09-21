using System;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public class LogOperacao
    {
        public LogOperacao(Guid idLogOperacao, Guid idUsuario, TipoOperacao tipoOperacao, DateTime data)
        {
            ValidateDomain(idLogOperacao, idUsuario, tipoOperacao, data);
        }

        public LogOperacao()
        {

        }

        private const string IdLogOperacaoInvalido = "IdLogOperação inválido!";
        private const string IdUsuarioInvalido = "IdUsuario inválido!";
        private const string DataInvalida = "Data log operação inválida!";
        private void ValidateDomain(Guid idLogOperacao, Guid idUsuario, TipoOperacao tipoOperacao, DateTime data)
        {
            DomainValidadorException.Whem(idLogOperacao == Guid.Empty, IdLogOperacaoInvalido);
            IdLogOperacao = idLogOperacao;

            DomainValidadorException.Whem(idUsuario == Guid.Empty, IdUsuarioInvalido);
            IdUsuario = idUsuario;

            TipoOperacao = tipoOperacao;

            DomainValidadorException.Whem(data.Day != DateTime.Now.Day, DataInvalida);
            Data = data;
        }

        public Guid IdLogOperacao { get; private set; }
        public Guid IdUsuario { get; private set; }
        public TipoOperacao TipoOperacao { get; private set; }
        public DateTime Data { get; private set; }
        public Usuario Usuario { get; set; }
    }
}
