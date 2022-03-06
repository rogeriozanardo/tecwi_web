using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.Repositories.Utils;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Data.Repositories
{
    public class PedidoMercoCampRepository : HelperRepository, IPedidoMercoCampRepository
    {
        private readonly DataContext _DataContext;

        public PedidoMercoCampRepository(DataContext dataContext,
                                         IConfiguration configuration)
            : base(configuration)
        {
            _DataContext = dataContext;
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
                    ID = pedidoMercoCamp.IdPedidoMercoCamp,
                    NumeroPedidoCliente = pedidoMercoCamp.NumPedido.ToString(),
                    CNPJEmitente = cnpjEmitente,
                    Sincronizacao = pedidoMercoCamp.DataEnvio
                };

                pedidoMercoCampDTO.Itens = new List<ConfirmacaoPedidoItemDTO>();
                foreach (var item in pedidoMercoCamp.PedidoItens)
                {
                    pedidoMercoCampDTO.Itens.Add(new ConfirmacaoPedidoItemDTO
                    { 
                        CodigoProduto = item.CdProduto,
                        Quantidade = item.Qtd.ToString(),
                        Sequencia = item.SeqTransmissao.ToString(),
                        QuantidadeConferida = item.Qtd.ToString()
                    });
                }
                
                pedidosMercoCampDTO.Add(pedidoMercoCampDTO);
            }

            return pedidosMercoCampDTO;
        }

        public async Task AtualizarStatusPedidosMercoCamp(List<ConfirmacaoPedidoDTO> pedidosDTO)
        {
            pedidosDTO = pedidosDTO ?? new List<ConfirmacaoPedidoDTO>();

            if (!pedidosDTO.Any())
                return;

            List<PedidoMercoCamp> pedidosMercoCamp = new List<PedidoMercoCamp>();
            foreach (var pedido in pedidosDTO)
            {
                pedidosMercoCamp.Add(new PedidoMercoCamp
                { 
                    IdPedidoMercoCamp = pedido.ID,
                    Status = StatusPedidoMercoCamp.Separado
                });
            }

             _DataContext.PedidoMercoCamp.UpdateRange(pedidosMercoCamp);
            await _DataContext.SaveChangesAsync();
        }
    }
}
