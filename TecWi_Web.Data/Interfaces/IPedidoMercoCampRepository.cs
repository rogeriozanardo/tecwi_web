﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Data.Interfaces
{
    public interface IPedidoMercoCampRepository
    {
        Task Inserir(PedidoMercoCamp pedidoMercoCamp);
        Task AtualizarStatusPedidosMercoCamp(CORPEMWMSCONFSEPDTO pedidosDTO);
    }
}
