using System;
using System.Collections.Generic;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public class Usuario
    {
        public Guid IdUsuario { get; private set; }
        public bool Ativo { get; set; } = true;
        public string Login { get; private  set; }
        public string Nome { get; private  set; }
        public string Email { get; private set; }
        public byte[] SenhaSalt { get; private set; }
        public byte[] SenhaHash { get; private set; }
        public List<UsuarioAplicacao> UsuarioAplicacao { get;set; }
        public List<Cliente> Cliente { get; set; }
        public List<ContatoCobranca> ContatoCobranca { get; set; }
        public List<ContatoCobrancaLancamento> ContatoCobrancaLancamento { get; set; }
        public List<LogOperacao> LogOperacao { get; set; }

        public Usuario(Guid idUsuario, string login, string nome, string eMail, byte[] senhaHash, byte[] senhaSalt)
        {
            ValidateDomain(idUsuario, login, nome, eMail, senhaHash, senhaSalt);
        }

        public Usuario()
        {
                
        }

        private string IdInvalido = "Id inválido!";
        private string LoginInvalido = "Login inválido!";
        private string NomeInvalido = "Nome inválido!";
        private string EmailInvalido = "Email inválido!";
        private string SenhaHashInvalido = "Senha hash inválido!";
        private string SenhaSaltInvalido = "Senha salt inválido!";
        private void ValidateDomain(Guid idUsuario, string login, string nome, string eMail, byte[] senhaHash, byte[] senhaSalt)
        {
            DomainValidadorException.Whem(idUsuario == Guid.Empty, IdInvalido);
            IdUsuario = idUsuario;

            DomainValidadorException.Whem(string.IsNullOrWhiteSpace(login), LoginInvalido);
            Login = login;

            DomainValidadorException.Whem(string.IsNullOrWhiteSpace(nome), NomeInvalido);
            Nome = nome;

            bool isValidEmail = !string.IsNullOrWhiteSpace(eMail) &&  ValidateEmail.IsValidEmail(eMail);
            DomainValidadorException.Whem(!isValidEmail, EmailInvalido);
            Email = eMail;

            DomainValidadorException.Whem(senhaHash == null || senhaHash.Length == 0, SenhaHashInvalido);
            SenhaHash = senhaHash;

            DomainValidadorException.Whem(senhaSalt == null || senhaSalt.Length == 0, SenhaSaltInvalido);
            SenhaSalt = senhaSalt;
        } 

        public void Update(Guid idUsuario, string login, string nome, string eMail, byte[] senhaHash, byte[] senhaSalt)
        {
            ValidateDomain(idUsuario, login, nome, eMail, senhaHash, senhaSalt);
        }
    }
}