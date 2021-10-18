using System;

namespace TecWi_Web.Shared.DTOs
{
    public class ContatoCobrancaLancamentoDTO
    {
        public Guid IdContato { get; set; }

        public Guid Idusuario { get; set; }

        public string Numlancto { get; set; }
        public int Sq { get; set; }
        public string CdFilial { get; set; }
        public UsuarioDTO UsuarioDTO { get; set; }
    }
}
