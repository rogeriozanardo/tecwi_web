using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;
using TecWi_Web.Data.Interfaces;
using TecWi_Web.Data.Repositories.Utils;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Domain.Exceptions;
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

        public async Task AtualizarStatusPedidosMercoCamp(CORPEM_WMS_CONF_SEP pedidoDTO)
        {
            if (pedidoDTO == null || string.IsNullOrEmpty(pedidoDTO.NUMPEDCLI))
                return;

            var pedidoMercoCamp = await _DataContext.PedidoMercoCamp
                                                    .Include(x => x.PedidoItens)
                                                    .FirstOrDefaultAsync(t => t.CdFilial == "ES" && t.NumPedido == Convert.ToInt32(pedidoDTO.NUMPEDCLI));

            if (pedidoMercoCamp == null)
                throw new PedidoNaoEncontradoException($"Pedido: {pedidoDTO.NUMPEDCLI} não encontrado no banco de dados para a filial ES");

            foreach (var item in pedidoMercoCamp.PedidoItens)
            {
               var itemPedido = pedidoMercoCamp.PedidoItens.FirstOrDefault(t => t.CdProduto == item.CdProduto && t.SeqTransmissao == item.SeqTransmissao);
                if (itemPedido == null)
                    continue;

                itemPedido.QtdSeparado = item.QtdSeparado;
            }

            bool itensEmAberto = pedidoMercoCamp.PedidoItens.Any(x => x.QtdSeparado > 0 && x.Qtd != x.QtdSeparado);
            pedidoMercoCamp.Status = itensEmAberto ? StatusPedidoMercoCamp.SeparadoParcial : StatusPedidoMercoCamp.Separado;
            await _DataContext.SaveChangesAsync();
        }
    }
}
