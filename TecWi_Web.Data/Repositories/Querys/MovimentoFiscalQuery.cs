namespace TecWi_Web.Data.Repositories.Querys
{
    public class MovimentoFiscalQuery
    {
        public static string QUERY_MOVIMENTO_FISCAL =
			@"
                select 
						PE.NumPedido as NumMovimento,
						n.numnota as NumeroNota, 
						n.serie as Serie,
						n.DataEmissao as DataEmissao,
						n.VlTotNF as ValorTotal,
						n.QtdVolume as QtdVolume,
						n.nfef_chaveacesso as ChaveAcesso,
						n.CdFilial as CdFilial,
						PVI.nummovimento as NumMovItem,
						PVI.seq as Sequencia,
						PVI.cdproduto as CdProduto,
						PVI.qtdsolicitada as Qtd
						from NtFiscal n WITH (NOLOCK)
				inner join pedido PE WITH (NOLOCK) on PE.NumPedido = n.NumPedido
				inner join pedidoitem PEI WITH (NOLOCK) on PE.NumPedido = PEI.numpedido
				inner join pendenciavendaitem PVI WITH (NOLOCK) on PVI.seq = PEI.seq
				where PE.NumPedido IN @NUM_MOVIMENTOS
            ";
    }
}
