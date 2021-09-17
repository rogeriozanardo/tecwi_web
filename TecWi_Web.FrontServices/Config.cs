using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.FrontServices
{
    public static class Config
    {
        public static string ApiUrl { get; set; }
        public static bool Autorizado { get; set; }
        public static UsuarioDTO usuarioDTO { get; set; }

    }
}
