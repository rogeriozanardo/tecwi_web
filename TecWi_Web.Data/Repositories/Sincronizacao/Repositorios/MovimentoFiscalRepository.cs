using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Dapper;
using TecWi_Web.Data.Repositories.Querys;
using TecWi_Web.Data.Repositories.Sincronizacao.Interfaces;
using TecWi_Web.Data.Repositories.Utils;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;
using EFCore.BulkExtensions;
using System;

namespace TecWi_Web.Data.Repositories.Sincronizacao.Repositorios
{
    public class MovimentoFiscalRepository : HelperRepository, IMovimentoFiscalRepository
    {
        private readonly IDapper _Dapper;
        private readonly DataContext _DataContext;

        public MovimentoFiscalRepository(IDapper dapper,
                                         DataContext dataContext,
                                         IConfiguration configuration)
            : base(configuration)
        {
            _Dapper = dapper;
            _DataContext = dataContext;
        }


        public async Task Inserir(List<MovimentoFiscal> movimentosFiscais)
        {
            await _DataContext.MovimentoFiscal.AddRangeAsync(movimentosFiscais);
            await _DataContext.SaveChangesAsync();
        }

        public async Task<List<MovimentoFiscalDTO>> ListarMovimentosFiscaisPendenteTransmissaoMercoCamp()
        {
            var movimentosFiscais = await _DataContext.MovimentoFiscal
                                                .Include(x => x.ItensMovimentoFiscal)
                                                .Where(x => x.Status == Domain.Enums.StatusTransmissaoMovimentoFiscal.PENDENTE)
                                                .ToListAsync();

            if (movimentosFiscais.Count() == 0)
                return null;

            List<MovimentoFiscalDTO> movimentosFiscaisDTO = new List<MovimentoFiscalDTO>();
            foreach (var movimentoFiscal in movimentosFiscais)
            {
                MovimentoFiscalDTO movimentoFiscalDTO = new MovimentoFiscalDTO
                {
                    ID = movimentoFiscal.ID,
                    ChaveNF = movimentoFiscal.ChaveAcesso.Trim(),
                    CNPJEmitente = RetornarCNPJPorFilial(movimentoFiscal.CdFilial),
                    DataEmissao = movimentoFiscal.DataEmissao.ToString("dd/MM/yyyy"),
                    NumNotaFiscal = movimentoFiscal.NumeroNota.ToString(),
                    NumPedidoCliente = movimentoFiscal.NumMovimento.ToString(),
                    QtdVolume = ((int)movimentoFiscal.QtdVolume).ToString(),
                    SerieNotaFiscal = movimentoFiscal.Serie.Trim(),
                    ValorTotalNota = decimal.Round(movimentoFiscal.ValorTotal, 2).ToString().Replace(",", ".")
                };

                foreach (var itemMovFiscal in movimentoFiscal.ItensMovimentoFiscal)
                {
                    movimentoFiscalDTO.ItensMovimentoFiscal.Add(new MovimentoFiscalItemDTO
                    {
                        CodigoProduto = itemMovFiscal.CdProduto,
                        Quantidade = ((int)itemMovFiscal.Qtd).ToString(),
                        Sequencia = itemMovFiscal.Sequencia.ToString()
                    });
                }
                movimentosFiscaisDTO.Add(movimentoFiscalDTO);
            }

            return movimentosFiscaisDTO;
        }

        public async Task Sincronizar()
        {
            var pedidos = await _DataContext.PedidoMercoCamp.Where(t => t.Status == StatusPedidoMercoCamp.Separado &&
                                                                        !(_DataContext.MovimentoFiscal.Any(x => x.NumMovimento == t.NumPedido)))
                                                            .ToListAsync();
            if (pedidos.Count() == 0)
                return;

            var connectionStringFilial01 = BuscarConnectionStringFilial01();
            var connectionStringFilial02 = BuscarConnectionStringFilial02();
            var connectionStringMatriz = BuscarConnectionStringMatriz();

            //string codigoPedidos = string.Join(",", pedidos.Select(x => x.NumPedido));
            int[] codigoPedidos = pedidos.Select(x => x.NumPedido).ToArray();
            var movimentosFiscaisFilial01 = await BuscarMovimentosFiscaisPorEmpresa(connectionStringFilial01, codigoPedidos);
            var movimentosFiscaisFilial02 = await BuscarMovimentosFiscaisPorEmpresa(connectionStringFilial02, codigoPedidos);
            var movimentosFiscaisMatriz = await BuscarMovimentosFiscaisPorEmpresa(connectionStringMatriz, codigoPedidos);

            if (movimentosFiscaisFilial01.Any())
                await InserirLoteMovimentosFiscais(movimentosFiscaisFilial01.ToList());

            if (movimentosFiscaisFilial02.Any())
                await InserirLoteMovimentosFiscais(movimentosFiscaisFilial02.ToList());

            if (movimentosFiscaisMatriz.Any())
                await InserirLoteMovimentosFiscais(movimentosFiscaisMatriz.ToList());
        }

        public async Task Atualizar(List<MovimentoFiscal> movimentosFiscais)
        {
            await _DataContext.BulkUpdateAsync<MovimentoFiscal>(movimentosFiscais, new BulkConfig
            {
                PropertiesToInclude = new List<string> { nameof(MovimentoFiscal.Status) }
            });
        }

        private async Task<IEnumerable<MovimentoFiscal>> BuscarMovimentosFiscaisPorEmpresa(string connectionString, int[] pedidos)
        {
            DynamicParameters parameters = new DynamicParameters();
            string sql = MovimentoFiscalQuery.QUERY_MOVIMENTO_FISCAL;
            var movimentosFiscaisTemp = new Dictionary<int, MovimentoFiscal>();

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var movimentoFiscal = await db.QueryAsync<MovimentoFiscal, MovimentoFiscalItem, MovimentoFiscal>(sql, (movFiscal, itemMovFiscal) =>
                {
                    MovimentoFiscal movFiscalTemp;

                    if (!movimentosFiscaisTemp.TryGetValue(movFiscal.NumMovimento, out movFiscalTemp))
                    {
                        movFiscalTemp = movFiscal;
                        movFiscalTemp.ItensMovimentoFiscal = new List<MovimentoFiscalItem>();
                        movimentosFiscaisTemp.Add(movFiscal.NumMovimento, movFiscalTemp);
                    }

                    if (!(movFiscalTemp.ItensMovimentoFiscal.Any(t => t.CdProduto == itemMovFiscal.CdProduto && t.Sequencia == itemMovFiscal.Sequencia)))
                    {
                        itemMovFiscal.MovimentoFiscal = movFiscalTemp;
                        movFiscalTemp.ItensMovimentoFiscal.Add(itemMovFiscal);
                    }


                    return movFiscalTemp;
                },
                splitOn: "NumMovItem", param: new { NUM_MOVIMENTOS = pedidos });
                return movimentoFiscal.ToList().Distinct();
            }
        }

        private async Task InserirLoteMovimentosFiscais(IEnumerable<MovimentoFiscal> movimentosFiscais)
        {
            await _DataContext.BulkInsertOrUpdateAsync<MovimentoFiscal>(movimentosFiscais.ToList(), opt =>
            {
                opt.UpdateByProperties = new List<string>() { "NumMovimento", "CdFilial", "NumeroNota", "Serie" };
                opt.PropertiesToIncludeOnUpdate = new List<string> { "" };
                opt.SetOutputIdentity = true;
            });
            var pedidosItens = movimentosFiscais.Where(x => x.ID > 0).SelectMany(x => x.ItensMovimentoFiscal).ToList();

            if (pedidosItens.Any())
            {
                pedidosItens.ForEach(item => item.IDMovimentoFiscal = item.MovimentoFiscal.ID);
                await _DataContext.BulkInsertOrUpdateAsync<MovimentoFiscalItem>(pedidosItens, opt =>
                {
                    opt.UpdateByProperties = new List<string>() { "CdProduto", "Sequencia", "Qtd", "IDMovimentoFiscal" };
                    opt.PropertiesToIncludeOnUpdate = new List<string> { "" };
                });
            }
        }
    }
}
