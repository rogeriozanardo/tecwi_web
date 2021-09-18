using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWi_Web.Shared.DTOs
{
    public class MensagemInformativaDTO
    {
        public bool Exibe { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
    }
}
