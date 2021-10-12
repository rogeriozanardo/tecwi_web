using System;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public class ClienteContato
    {
        public ClienteContato()
        {

        }

        public ClienteContato(Guid idClienteContato, int cdclifor, string nome, string telefone, string email)
        {
            ValidateDomain(idClienteContato, cdclifor, nome, telefone, email);
        }

        public void Update(Guid idClienteContato, int cdclifor, string nome, string telefone, string email)
        {
            ValidateDomain(idClienteContato, cdclifor, nome, telefone, email);
        }

        public Guid IdClienteContato { get; private set; }
        public int Cdclifor { get; private set; }
        public string Nome { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public Cliente Cliente { get; set; }

        public string IdClienteContatoInvalido = "IdClienteContato inválido!";
        public string CdcliforInvalido = "Cdclifor inválido!";
        public string NomeInvalido = "Nome inválido!";
        public string TelefoneInvalido = "Telefone inválido!";
        public string EmailInvalido = "Email inválido!";

        private void ValidateDomain(Guid idClienteContato, int cdclifor, string nome, string telefone, string email)
        {
            DomainValidadorException.Whem(idClienteContato == Guid.Empty, IdClienteContatoInvalido);
            IdClienteContato = idClienteContato;

            DomainValidadorException.Whem(cdclifor == 0, CdcliforInvalido);
            Cdclifor = cdclifor;

            DomainValidadorException.Whem(string.IsNullOrWhiteSpace(nome), NomeInvalido);
            Nome = nome;

            DomainValidadorException.Whem(string.IsNullOrWhiteSpace(telefone), TelefoneInvalido);
            Telefone = telefone;

            bool isValidEmail = !string.IsNullOrWhiteSpace(email) && ValidateEmail.IsValidEmail(email);
            DomainValidadorException.Whem(!isValidEmail, EmailInvalido);
            Email = email;
        }
    }
}
