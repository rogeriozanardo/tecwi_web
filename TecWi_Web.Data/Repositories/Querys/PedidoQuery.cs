namespace TecWi_Web.Data.Repositories.Querys
{
    public class PedidoQuery
    {
		public static string QUERY_SELECT_PEDIDOS_E_ITENS =
		   @"
               SELECT 
						CONCAT(P.cdempresa, P.cdfilial, P.nummovimento) AS PedidoId,
						P.cdempresa, 
						P.cdfilial, 
						P.cdcliente, 
						P.nummovimento,
						P.nummovcliente,
						P.dtinicio, 
						P.stpendencia,
						P.cdvendedor, 
						P.stpagafrete, 
						P.cdtransportadora,
						P.cdtransportadoraredespacho, 
						P.stpagafreteredespacho,
						CONCAT(PI.cdempresa, PI.cdfilial, PI.nummovimento, PI.seq) AS PedidoItemId,  
						PI.cdempresa, 
						PI.cdfilial, 
						PI.nummovimento, 
						PI.seq, 
						PI.cdproduto, 
						PI.tpregistro, 
						PI.qtdsolicitada, 
						PI.qtdprocessada,
						PI.VlVenda, 
						PI.vlcalculado, 
						PI.uporigem
			  FROM pendenciavenda1 P WITH (NOLOCK)
			  INNER JOIN pendenciavendaitem PI WITH (NOLOCK) ON PI.cdempresa = P.cdempresa 
					AND PI.cdfilial = P.cdfilial 
					AND PI.nummovimento = P.nummovimento 
			  WHERE stpendencia='F' 
            ";

		public static string QUERY_SELECT_PEDIDOS_COMPLETO =
			@"
               SELECT 
						CONCAT(P.cdempresa, P.cdfilial, P.nummovimento) AS PedidoId,
						P.cdempresa, 
						P.cdfilial, 
						P.cdcliente, 
						P.nummovimento,
						P.nummovcliente,
						P.dtinicio, 
						P.stpendencia,
						P.cdvendedor, 
						P.stpagafrete, 
						P.cdtransportadora,
						P.cdtransportadoraredespacho, 
						P.stpagafreteredespacho,
						CONCAT(PI.cdempresa, PI.cdfilial, PI.nummovimento, PI.seq) AS PedidoItemId,  
						PI.cdempresa, 
						PI.cdfilial, 
						PI.nummovimento, 
						PI.seq, 
						PI.cdproduto, 
						PI.tpregistro, 
						PI.qtdsolicitada, 
						PI.qtdprocessada,
						PI.VlVenda, 
						PI.vlcalculado, 
						PI.uporigem,
						E.empresaid AS EmpresaId,
						C.cdcliente AS CdCliente,
						E.Razao,
						E.Fantasia,
						E.Email,
						E.StAtivo,
						E.UpdRegistro,
						E.Cep,
						E.Cidade,
						E.Endereco,
						E.Complemento,
						E.Bairro,
						E.DDD,
						E.Fone1,
						E.Fone2,
						E.UF,
						E.InscriEst,
						E.InscriFed,
						T.CdTransportadora, 
						T.Inscrifed,
						T.Tpinscricao,
						T.Nome,
						T.Fantasia,
						T.Cidade,
						T.updregistro,
						T.stativo,
						T.email,
						V.CdVendedor,
						V.Apelido,
						V.Nome,
						V.UpdRegistro
			  FROM pendenciavenda1 P WITH (NOLOCK)
			  INNER JOIN pendenciavendaitem PI WITH (NOLOCK) ON PI.cdempresa = P.cdempresa 
					AND PI.cdfilial = P.cdfilial 
					AND PI.nummovimento = P.nummovimento 
				INNER JOIN cliente C WITH (NOLOCK) on C.cdcliente = P.cdcliente
				INNER JOIN empresa E WITH (NOLOCK) on C.empresaid = E.empresaid  
				INNER JOIN vendedor V WITH (NOLOCK) on V.CdVendedor = P.cdvendedor
				LEFT JOIN transportadora t on t.cdtransportadora = P.cdtransportadora
			  WHERE stpendencia='F' 
            ";

		public static string QUERY_SELECT_PEDIDOS_ENCERRADOS =
			@"
				 SELECT 
						P.cdempresa, 
						P.cdfilial, 
						P.cdcliente, 
						P.nummovimento,
						P.nummovcliente,
						P.dtinicio, 
						P.stpendencia,
						P.cdvendedor, 
						P.stpagafrete, 
						P.cdtransportadora,
						P.cdtransportadoraredespacho, 
						P.stpagafreteredespacho
			  FROM pendenciavenda1 P WITH (NOLOCK)
				WHERE P.stpendencia = 'E' 
				AND P.nummovimento IN @NUM_MOVIMENTOS
			";
	}
}
