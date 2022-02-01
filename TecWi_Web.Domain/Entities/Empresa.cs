using System;

namespace TecWi_Web.Domain.Entities
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        public string CdCliente { get; set; }
        public string Razao { get; set; }
        public string Fantasia { get; set; }
        public string Email { get; set; }
        public int StAtivo { get; set; }
        public DateTime? UpdRegistro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Endereco { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string DDD { get; set; }
        public string Fone1 { get; set; }
        public string Fone2 { get; set; }
        public string UF { get; set; }
        public string InscriEst { get; set; }
        public string InscriFed { get; set; }
    }
}
