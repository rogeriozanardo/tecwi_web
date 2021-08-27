using System;

namespace TecWi_Web.Domain.Entities
{
    public class Usuario
    {
        public Guid IdUsuario { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public byte[] SenhaSalt { get; set; }
        public byte[] SenhaHash { get; set; }
    }
}
