using System;

namespace TecWi_Web.Shared.DTOs
{
    public class ClienteContatoDTO
    {
        public Guid IdClienteContato { get; set; } 
        public int Cdclifor { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
}