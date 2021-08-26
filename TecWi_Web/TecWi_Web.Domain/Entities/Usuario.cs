using System;
using System.Collections.Generic;
using System.Text;

namespace TecWi_Web.Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public byte[] SenhaSalt { get; set; }
        public byte[] SenhaHash { get; set; }
    }
}
