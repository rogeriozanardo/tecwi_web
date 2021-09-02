using System.Collections.Generic;
using TecWi_Web.Domain.Validators;

namespace TecWi_Web.Domain.Entities
{
    public class Cliente
    {
        public Cliente(int cdclifor, string inscrifed, string fantasia, string razao, string ddd, string fone1, string fone2, string email, string cidade)
        {
            ValidateDomain(cdclifor, inscrifed, fantasia, razao, ddd, fone1, fone2, email, cidade);
        }

        public Cliente()
        {

        }

        public int Cdclifor { get; private set; }
        public string Inscrifed { get; private set; }
        public string Fantasia { get; private set; }
        public string Razao { get; private set; }
        public string DDD { get; private set; }
        public string Fone1 { get; private set; }
        public string Fone2 { get; private set; }
        public string Email { get; private set; }
        public string Cidade { get; private set; }
        public List<PagarReceber> PagarReceber { get; set; }

        private string CdcliforInvalido = "Campo 'Cdclifor' inválido!";
        private void ValidateDomain(int cdclifor, string inscrifed, string fantasia, string razao, string ddd, string fone1, string fone2, string email, string cidade)
        {
            DomainValidadorException.Whem(cdclifor == 0, CdcliforInvalido);
            Cdclifor = cdclifor;

            Inscrifed = inscrifed;

            Fantasia = fantasia;

            Razao = razao;

            DDD = ddd;

            Fone1 = fone1;

            Fone2 = fone2;

            Email = email;

            Cidade = cidade;
        }
    }
}
