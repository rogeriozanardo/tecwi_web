using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Repositories.Querys;
using TecWi_Web.Data.Repositories.Sincronizacao.Interfaces;
using TecWi_Web.Domain.Entities;
using EFCore.BulkExtensions;
using TecWi_Web.Shared.DTOs;
using System;
using Microsoft.EntityFrameworkCore;
using TecWi_Web.Data.ValueObjects;
using TecWi_Web.Data.Repositories.Utils;

namespace TecWi_Web.Data.Repositories.Sincronizacao.Repositorios
{
    public class PedidoSincronizacaoRepository : HelperRepository, IPedidoSincronizacaoRepository
    {
        private readonly DataContext _DataContext;
        const string STATUS_FECHADO = "F";
        const string STATUS_ENCERRADO = "E";
        const string CODIGO_FILIAL_01_BAHIA = "BA";
        const string CODIGO_FILIAL_02_ESPIRITO_SANTO = "ES";
        const string CODIGO_MATRIZ = "SP";
        const string TIPO_FRETE_CIF = "C";
        const int SEQUENCIA_ENVIO_INICIAL = 1;

        public PedidoSincronizacaoRepository(DataContext dataContext,
                                             IConfiguration configuration)
            : base(configuration)
        {
            _DataContext = dataContext;
        }

        public async Task Sincronizar()
        {
            var connectionStringFilial01 = BuscarConnectionStringFilial01();
            var connectionStringFilial02 = BuscarConnectionStringFilial02();
            var connectionStringMatriz = BuscarConnectionStringMatriz();

            List<Empresa> empresas = new List<Empresa>();
            List<Transportadora> transportadoras = new List<Transportadora>();
            List<Vendedor> vendedores = new List<Vendedor>();
            List<Pedido> pedidos = new List<Pedido>();

            var sincronizacaoPedidoMatriz = await BuscarPedidos(connectionStringMatriz);
            PopularDadosSincronizacaoPedidos(sincronizacaoPedidoMatriz, pedidos, empresas, transportadoras, vendedores);

            var pedidosFilial01 = await BuscarPedidos(connectionStringFilial01);
            PopularDadosSincronizacaoPedidos(pedidosFilial01, pedidos, empresas, transportadoras, vendedores);

            var pedidosFilial02 = await BuscarPedidos(connectionStringFilial02);
            PopularDadosSincronizacaoPedidos(pedidosFilial02, pedidos, empresas, transportadoras, vendedores);

            if (pedidos != null && pedidos.Any())
            {
                using (var transaction = _DataContext.Database.BeginTransaction())
                {
                    await _DataContext.BulkInsertOrUpdateAsync<Pedido>(pedidos, opt =>
                    {
                        opt.UpdateByProperties = new List<string>() { "cdempresa", "cdfilial", "nummovimento" };
                        opt.PropertiesToIncludeOnUpdate = new List<string> { "" };
                        opt.SetOutputIdentity = true;
                    });
                    var pedidosItens = pedidos.Where(x => x.ID > 0).SelectMany(x => x.PedidoItem).ToList();

                    if (pedidosItens.Any())
                    {
                        pedidosItens.ForEach(item => item.IDPedido = item.Pedido.ID);
                        await _DataContext.BulkInsertOrUpdateAsync<PedidoItem>(pedidosItens, opt =>
                        {
                            opt.UpdateByProperties = new List<string>() { "cdempresa", "cdfilial", "nummovimento", "seq" };
                            opt.PropertiesToIncludeOnUpdate = new List<string> { "" };
                        });
                    }

                    if (transportadoras.Any())
                    {
                        await _DataContext.BulkInsertOrUpdateAsync<Transportadora>(transportadoras, opt =>
                        {
                            opt.UpdateByProperties = new List<string>() { "CdTransportadora" };
                            opt.PropertiesToIncludeOnUpdate = new List<string> { "" };
                        });
                    }

                    if (vendedores.Any())
                    {
                        await _DataContext.BulkInsertOrUpdateAsync<Vendedor>(vendedores, opt =>
                        {
                            opt.UpdateByProperties = new List<string>() { "CdVendedor" };
                            opt.PropertiesToIncludeOnUpdate = new List<string> { "" };
                        });
                    }

                    if (empresas.Any())
                    {
                        await _DataContext.BulkInsertOrUpdateAsync<Empresa>(empresas, opt =>
                        {
                            opt.UpdateByProperties = new List<string>() { "EmpresaId" };
                            opt.PropertiesToIncludeOnUpdate = new List<string> { "" };
                        });
                    }

                    transaction.Commit();
                }
            }
        }

        public async Task AlterarStatusPedidoFaturadoEncerrado()
        {
            var pedidosStatusFaturado = _DataContext.Pedido.Where(t => t.stpendencia == STATUS_FECHADO).ToList();
            if (pedidosStatusFaturado.Count() == 0)
                return;

            var connectionStringFilial01 = BuscarConnectionStringFilial01();
            var connectionStringFilial02 = BuscarConnectionStringFilial02();
            var connectionStringMatriz = BuscarConnectionStringMatriz();

            List<Pedido> pedidosEncerrados = new List<Pedido>();
            var pedidosFilialBahia = await BuscarPedidosEncerradosPorFilial(connectionStringFilial01, CODIGO_FILIAL_01_BAHIA, pedidosStatusFaturado);
            if (pedidosFilialBahia != null && pedidosFilialBahia.Any())
                pedidosEncerrados.AddRange(pedidosFilialBahia);

            var pedidosFilialEspiritoSanto = await BuscarPedidosEncerradosPorFilial(connectionStringFilial02, CODIGO_FILIAL_02_ESPIRITO_SANTO, pedidosStatusFaturado);
            if (pedidosFilialEspiritoSanto != null && pedidosFilialEspiritoSanto.Any())
                pedidosEncerrados.AddRange(pedidosFilialEspiritoSanto);

            var pedidosMatriz = await BuscarPedidosEncerradosPorFilial(connectionStringMatriz, CODIGO_MATRIZ, pedidosStatusFaturado);
            if (pedidosMatriz != null && pedidosMatriz.Any())
                pedidosEncerrados.AddRange(pedidosMatriz);

            if (pedidosEncerrados.Count == 0)
                return;

            pedidosEncerrados.ForEach(t => t.stpendencia = STATUS_ENCERRADO);
            await _DataContext.BulkUpdateAsync<Pedido>(pedidosEncerrados);
        }

        public async Task<List<PedidoMercoCampDTO>> ListarPedidosNaoEnviadosMercoCamp()
        {
            var pedidos = await _DataContext.PedidoItem
                                .Include(t => t.Pedido)
                                .Where(t => (t.uporigem == "ES" || t.uporigem.StartsWith("ES"))
                                            && t.Pedido.stpendencia == STATUS_FECHADO 
                                            && t.Pedido.cdfilial.Trim() == CODIGO_FILIAL_02_ESPIRITO_SANTO &&
                                            !(_DataContext.PedidoMercoCamp.Any(x => x.NumPedido == t.nummovimento)))
                                .ToListAsync();

            List<PedidoMercoCampDTO> pedidosMercoCampDTO = new List<PedidoMercoCampDTO>();
            foreach (var pedido in pedidos)
            {
                var empresa = await _DataContext.Empresa.FirstOrDefaultAsync(x => x.CdCliente == pedido.Pedido.cdcliente);
                var transportadora = await _DataContext.Transportadora.FirstOrDefaultAsync(x => x.CdTransportadora == pedido.Pedido.cdtransportadora);
                var vendedor = await _DataContext.Vendedor.FirstOrDefaultAsync(x => x.CdVendedor == pedido.Pedido.cdvendedor);

                string cnpjEmitente = RetornarCNPJPorFilial(pedido.cdfilial);
                var pedidoMercoCampDTO = new PedidoMercoCampDTO
                {
                    ID = pedido.ID,
                    DataInclusaoERP = pedido.Pedido.dtinicio.GetValueOrDefault(DateTime.Now),
                    ValorTotalPedido = pedido.Pedido.PedidoItem.Sum(t => t.vlcalculado.GetValueOrDefault(decimal.Zero)),
                    NumeroPedidoCliente = pedido.nummovimento.ToString(),
                    BairroDestinatario = empresa?.Bairro,
                    CepDestinatario = empresa?.Cep,
                    CNPJDestinatario = empresa?.InscriFed,
                    CNPJEmitente = cnpjEmitente,
                    CNPJTransportadora = transportadora?.Inscrifed,
                    CodigoVendedor = vendedor?.CdVendedor,
                    NomeVendedor = vendedor?.Apelido,
                    ComplementoDestinatario = empresa?.Complemento,
                    DataLiberacaoERP = DateTime.Now,
                    DataPrevisaoEntradaSite = DateTime.Now,
                    DDDRastro = empresa?.DDD,
                    EmailRastro = empresa?.Email,
                    LogradouroDestinatario = empresa?.Endereco,
                    MunicipioDestinatario = empresa?.Cidade,
                    NomeDestinatario = empresa?.Fantasia,
                    SequenciaEnvio = SEQUENCIA_ENVIO_INICIAL,
                    TelRastro = !string.IsNullOrEmpty(empresa?.Fone1) ? empresa?.Fone1 : empresa?.Fone2,
                    TipoFrete = TIPO_FRETE_CIF,
                    UFDestinatario = empresa?.UF,
                    CdFilial = pedido.cdfilial
                };

             
                    var quantidade = pedido.qtdsolicitada.GetValueOrDefault(decimal.Zero).ToString();
                    pedidoMercoCampDTO.Itens.Add(new PedidoItemMercoCampDTO
                    {
                        CodigoProduto = pedido.cdproduto,
                        Quantidade = quantidade.Length > 9 ? quantidade.Substring(0, 9) : quantidade,
                        Sequencia = pedido.seq.ToString(),
                        LoteFabricacao = string.Empty
                    });
                

                pedidosMercoCampDTO.Add(pedidoMercoCampDTO);
            }

            return pedidosMercoCampDTO;
        }

        #region Helpers

        private async Task<SincronizacaoPedidosViewObjects> BuscarPedidos(string connectionString)
        {
            SincronizacaoPedidosViewObjects sincronizacaoPedidos = new SincronizacaoPedidosViewObjects();
            string sql = PedidoQuery.QUERY_SELECT_PEDIDOS_COMPLETO;
            IEnumerable<Pedido> pedidos = new List<Pedido>();
            List<Empresa> empresas = new List<Empresa>();
            List<Vendedor> vendedores = new List<Vendedor>();
            List<Transportadora> transportadoras = new List<Transportadora>();

            var pedidosTemp = new Dictionary<string, Pedido>();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                pedidos = await db.QueryAsync<Pedido, PedidoItem, Empresa, Transportadora, Vendedor, Pedido>(sql, (pedido, item, empresa, transportadora, vendedor) =>
                  {
                      Pedido pedidoTemp;
                      string chave = string.Concat(pedido.cdempresa, pedido.cdfilial, pedido.nummovimento);
                      if (!pedidosTemp.TryGetValue(chave, out pedidoTemp))
                      {
                          pedidoTemp = pedido;
                          pedidoTemp.PedidoItem = new List<PedidoItem>();
                          pedidosTemp.Add(chave, pedidoTemp);
                      }

                      if (!string.IsNullOrEmpty(transportadora?.CdTransportadora) && !(transportadoras.Any(t => t.CdTransportadora == transportadora.CdTransportadora)))
                          transportadoras.Add(transportadora);

                      if (empresa != null && empresa.EmpresaId > 0 && !(empresas.Any(t => t.EmpresaId == empresa.EmpresaId && t.CdCliente == empresa.CdCliente)))
                          empresas.Add(empresa);

                      if (!string.IsNullOrEmpty(vendedor?.CdVendedor) && !(vendedores.Any(t => t.CdVendedor == vendedor.CdVendedor)))
                          vendedores.Add(vendedor);

                      item.Pedido = pedidoTemp;
                      pedidoTemp.PedidoItem.Add(item);
                      return pedidoTemp;
                  },
                splitOn: "PedidoId, PedidoItemId, EmpresaId, CdTransportadora, CdVendedor");
                pedidos = pedidos.ToList().Distinct();
            }

            sincronizacaoPedidos.Pedidos = pedidos;
            sincronizacaoPedidos.Empresas = empresas;
            sincronizacaoPedidos.Transportadoras = transportadoras;
            sincronizacaoPedidos.Vendedores = vendedores;

            return sincronizacaoPedidos;
        }

        

        private async Task<List<Pedido>> BuscarPedidosEncerradosPorFilial(string connectionString, string filial, List<Pedido> pedidos)
        {
            var numMovimentosPorFilial = pedidos.Where(t => t.cdfilial.Trim() == filial.Trim()).Select(t => t.nummovimento);
            if (numMovimentosPorFilial == null || !numMovimentosPorFilial.Any())
                return null;

            DynamicParameters parameters = new DynamicParameters();
            var args = new Dictionary<string, object>();
            args.Add("NUM_MOVIMENTOS", numMovimentosPorFilial);
            parameters.AddDynamicParams(args);

            string sql = PedidoQuery.QUERY_SELECT_PEDIDOS_ENCERRADOS;
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var pedidosEncerrados = await db.QueryAsync<Pedido>(sql, parameters);
                if (pedidosEncerrados != null && pedidosEncerrados.Any())
                {
                    var pedidosAtualizar = pedidos.Where(t => pedidosEncerrados.Any(x => x.nummovimento == t.nummovimento)).ToList();
                    if (pedidosAtualizar.Any())
                        return pedidosAtualizar.ToList();
                }
            }

            return null;
        }

        private void PopularDadosSincronizacaoPedidos(SincronizacaoPedidosViewObjects sincronizacaoPedido,
                                                      List<Pedido> pedidos,
                                                      List<Empresa> empresas,
                                                      List<Transportadora> transportadoras,
                                                      List<Vendedor> vendedores)
        {
            if (sincronizacaoPedido == null)
                return;

            if (sincronizacaoPedido.Pedidos != null && sincronizacaoPedido.Pedidos.Any())
                pedidos.AddRange(sincronizacaoPedido.Pedidos);

            if (sincronizacaoPedido.Empresas != null && sincronizacaoPedido.Empresas.Any())
            {
                var empresasNaoInseridas = sincronizacaoPedido.Empresas.Where(t => !(empresas.Any(x => x.CdCliente == t.CdCliente && x.EmpresaId == t.EmpresaId)));
                if (empresasNaoInseridas != null && empresasNaoInseridas.Any())
                    empresas.AddRange(empresasNaoInseridas);
            }

            if (sincronizacaoPedido.Transportadoras != null && sincronizacaoPedido.Transportadoras.Any())
            {
                var transportadorasNaoInseridas = sincronizacaoPedido.Transportadoras.Where(t => !(transportadoras.Any(x => x.CdTransportadora == t.CdTransportadora)));
                if (transportadorasNaoInseridas != null && transportadorasNaoInseridas.Any())
                    transportadoras.AddRange(transportadorasNaoInseridas);
            }

            if (sincronizacaoPedido.Vendedores != null && sincronizacaoPedido.Vendedores.Any())
            {
                var vendedoresNaoInseridos = sincronizacaoPedido.Vendedores.Where(t => !(vendedores.Any(x => x.CdVendedor == t.CdVendedor)));
                if (vendedoresNaoInseridos != null && vendedoresNaoInseridos.Any())
                    vendedores.AddRange(vendedoresNaoInseridos);
            }
        }

        #endregion
    }
}
