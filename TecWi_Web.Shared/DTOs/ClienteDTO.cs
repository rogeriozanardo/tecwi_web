using System.Collections.Generic;

namespace TecWi_Web.Shared.DTOs
{
    public class ClienteDTO
    {
        public int cdclifor { get; set; }
        public string inscrifed { get; set; }
        public string fantasia { get; set; }
        public string razao { get; set; }
        public string ddd { get; set; }
        public string fone1 { get; set; }
        public string fone2 { get; set; }
        public string email { get; set; }
        public string cidade { get; set; }
        public decimal totalLancamentos { get; set; }
        public UsuarioDTO UsuarioDTO { get; set; }
        public List<PagarReceberDTO> PagarReceberDTO { get; set; } = new List<PagarReceberDTO>();
        public List<ContatoCobrancaDTO> ContatoCobrancaDTO { get; set; } = new List<ContatoCobrancaDTO>();
        public List<ClienteContatoDTO> ClienteContatoDTO { get; set; } = new List<ClienteContatoDTO>();
    }
}
