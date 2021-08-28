﻿using System.Collections.Generic;

namespace TecWi_Web.Shared.DTOs
{
    public class UsuarioDTO
    {
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public List<UsuarioAplicacaoDTO> UsuarioAplicacaoDTO { get; set; }
    }
}
