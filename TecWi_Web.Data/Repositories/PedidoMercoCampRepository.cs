using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Data.Repositories
{
    public class PedidoMercoCampRepository : IPedidoMercoCampRepository
    {
        private readonly DataContext _DataContext;
        private readonly IConfiguration _Configuration;

        public PedidoMercoCampRepository(DataContext dataContext,
                                         IConfiguration configuration)
        {
            _DataContext = dataContext;
            _Configuration = configuration;
        }

        public async Task Inserir(PedidoMercoCamp pedidoMercoCamp)
        {
           await _DataContext.PedidoMercoCamp.AddAsync(pedidoMercoCamp);
           await _DataContext.SaveChangesAsync();
        }

        public async Task<List<ConfirmacaoPedidoDTO>> ListarPedidosSincronizarTransmitidosMercoCamp()
        {
            List<ConfirmacaoPedidoDTO> pedidosMercoCampDTO = new List<ConfirmacaoPedidoDTO>();
            var pedidosTransmitidos = await _DataContext.PedidoMercoCamp.Include(x => x.PedidoItens).Where(x => x.Status == StatusPedidoMercoCamp.Transmitido).ToListAsync();

            foreach (var pedidoMercoCamp in pedidosTransmitidos)
            {
                string cnpjEmitente = RetornarCNPJPorFilial(pedidoMercoCamp.CdFilial);
                ConfirmacaoPedidoDTO pedidoMercoCampDTO = new ConfirmacaoPedidoDTO
                { 
                    NumeroPedidoCliente = pedidoMercoCamp.NumPedido.ToString(),
                    CNPJEmitente = cnpjEmitente,
                    Sincronizacao = pedidoMercoCamp.DataEnvio
                };

                pedidoMercoCampDTO.Itens = new List<PedidoItemMercoCampDTO>();
                foreach (var item in pedidoMercoCamp.PedidoItens)
                {
                    pedidoMercoCampDTO.Itens.Add(new PedidoItemMercoCampDTO 
                    { 
                        CodigoProduto = item.CdProduto,
                        Quantidade = item.Qtd.ToString(),
                        Sequencia = item.SeqTransmissao.ToString(),
                        LoteFabricacao = string.Empty,
                        QuantidadeConferida = item.Qtd.ToString()
                    });
                }
                
                pedidosMercoCampDTO.Add(pedidoMercoCampDTO);
            }

            return pedidosMercoCampDTO;
        }


        private string RetornarCNPJPorFilial(string filial)
        {
            filial = filial ?? string.Empty;

#if DEBUG
            filial = "HOMOLOGACAO";
#endif

            filial = filial.Trim().ToUpper();

            switch (filial)
            {
                case "BA":
                    return _Configuration.GetSection("AppSettings").GetSection("CNPJ_Filial01").Value;
                case "ES":
                    return _Configuration.GetSection("AppSettings").GetSection("CNPJ_Filial02").Value;
                case "SP":
                    return _Configuration.GetSection("AppSettings").GetSection("CNPJ_Matriz").Value;
                case "HOMOLOGACAO":
                    return _Configuration.GetSection("AppSettings").GetSection("CNPJ_HOMOLOGACAO_TESTE").Value;
                default:
                    break;
            }


            return string.Empty;
        }
    }
}
