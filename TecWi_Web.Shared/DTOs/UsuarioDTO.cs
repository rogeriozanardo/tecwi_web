using System.Collections.Generic;

namespace TecWi_Web.Shared.DTOs
{
    public class UsuarioDTO
    {
        public string Login { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
        public List<UsuarioAplicacaoDTO> UsuarioAplicacaoDTO { get; set; }

        public UsuarioDTO()
        {
            this.UsuarioAplicacaoDTO = new List<UsuarioAplicacaoDTO>();
        }
    }
}
