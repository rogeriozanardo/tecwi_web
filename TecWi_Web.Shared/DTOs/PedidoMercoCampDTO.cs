using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class PedidoMercoCampDTO
    {
        [JsonIgnore]
        public int ID { get; set; }
        [JsonPropertyName("CGCEMINF")]
        public string CNPJEmitente { get; set; }
        [JsonPropertyName("OBSPED")]
        public string ObservacaoPedido { get; set; }
        [JsonPropertyName("OBSROM")]
        public string ObservacaoRomaneio { get; set; }
        [JsonPropertyName("NUMPEDCLI")]
        public string NumeroPedidoCliente { get; set; }
        [JsonPropertyName("NUMPEDRCA")]
        public string NumeroPedidoRCA { get; set; }
        [JsonPropertyName("VLTOTPED")]
        public decimal ValorTotalPedido { get; set; }
        [JsonPropertyName("CGCDEST")]
        public string CNPJDestinatario { get; set; }
        [JsonPropertyName("NOMEDEST")]
        public string NomeDestinatario { get; set; }
        [JsonPropertyName("CEPDEST")]
        public string CepDestinatario { get; set; }
        [JsonPropertyName("UFDEST")]
        public string UFDestinatario { get; set; }
        [JsonPropertyName("IBGEMUNDEST")]
        public string IBGEMunicipioDestinatario { get; set; }
        [JsonPropertyName("MUN_DEST")]
        public string MunicipioDestinatario { get; set; }
        [JsonPropertyName("BAIR_DEST")]
        public string BairroDestinatario { get; set; }
        [JsonPropertyName("LOGR_DEST")]
        public string LogradouroDestinatario { get; set; }
        [JsonPropertyName("NUM_DEST")]
        public string NumeroDestinatario { get; set; }
        [JsonPropertyName("COMP_DEST")]
        public string ComplementoDestinatario { get; set; }
        [JsonPropertyName("TP_FRETE")]
        public string TipoFrete { get; set; }
        [JsonPropertyName("CODVENDEDOR")]
        public string CodigoVendedor { get; set; }
        [JsonPropertyName("NOMEVENDEDOR")]
        public string NomeVendedor { get; set; }
        [JsonPropertyName("DTINCLUSAOERP")]
        public DateTime DataInclusaoERP { get; set; }
        [JsonPropertyName("DTLIBERACAOERP")]
        public DateTime DataLiberacaoERP { get; set; }
        [JsonPropertyName("DTPREV_ENT_SITE")]
        public DateTime DataPrevisaoEntradaSite { get; set; }
        [JsonPropertyName("EMAILRASTRO")]
        public string EmailRastro { get; set; }
        [JsonPropertyName("DDDRASTRO")]
        public string DDDRastro { get; set; }
        [JsonPropertyName("TELRASTRO")]
        public string TelRastro { get; set; }
        [JsonPropertyName("CGC_TRP")]
        public string CNPJTransportadora { get; set; }
        [JsonPropertyName("UF_TRP")]
        public string UFTransportadora { get; set; }
        [JsonPropertyName("PRIORIDADE")]
        public string Prioridade { get; set; }
        [JsonPropertyName("ETQCLIZPLBASE64")]
        public string Etiqueta { get; set; }

        [JsonPropertyName("ITENS")]
        public List<PedidoItemMercoCampDTO> Itens { get; set; }
        [JsonIgnore]
        public int SequenciaEnvio { get; set; }
        [JsonIgnore]
        public string CdFilial { get; set; }

        public PedidoMercoCampDTO()
        {
            Itens = new List<PedidoItemMercoCampDTO>();
        }
    }
}
