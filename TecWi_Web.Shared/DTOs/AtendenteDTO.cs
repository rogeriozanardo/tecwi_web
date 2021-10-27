using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Shared.DTOs
{
    public class AtendenteDTO
    {
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; }
        public int TotalContatos { get; set; }
        public List<ClienteDTO> clienteDTO { get; set; } = new List<ClienteDTO>();

    }
}
