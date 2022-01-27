using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TecWi_Web.Shared.DTOs
{
    public class ProdutoMercoCampDTO
    {
        public ProdutoMercoCampDTO()
        {
            Embalagens = new List<EmbalagemMercoCampDTO>();
        }
       
        [JsonPropertyName("CODPROD")]
        public string CdProduto { get; set; }
        [JsonPropertyName("NOMEPROD")]
        public string DsVenda { get; set; }
        [JsonPropertyName("IWS_ERP")]
        public string IndicadorWS { get; set; }
        [JsonPropertyName("TPOLRET")]
        public string PoliticaRetiradaMercadoria { get; set; }
        [JsonPropertyName("IAUTODTVEN")]
        public string DataVencimentoAutomatica { get; set; }
        [JsonPropertyName("QTDDPZOVEN")]
        public string QuantidadeDiasVencimentoAutomatica { get; set; }
        [JsonPropertyName("ILOTFAB")]
        public string ControlaLoteProduto { get; set; }
        [JsonPropertyName("IDTFAB")]
        public string ControlaDataFabricacaoProduto { get; set; }
        [JsonPropertyName("IDTVEN")]
        public string ControlaDataVencimentoProduto { get; set; }
        [JsonPropertyName("INSER")]
        public string ControlaNumeroSerieProduto { get; set; }
        [JsonPropertyName("CODFAB")]
        public string CodigoFornecedor { get; set; }
        [JsonPropertyName("NOMEFAB")]
        public string NomeFornecedor { get; set; }
        [JsonPropertyName("EMBALAGENS")]
        public List<EmbalagemMercoCampDTO> Embalagens { get; set; }
    }
}
