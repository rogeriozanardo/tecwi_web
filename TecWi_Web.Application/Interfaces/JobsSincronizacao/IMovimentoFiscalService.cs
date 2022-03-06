using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces.JobsSincronizacao
{
    public interface IMovimentoFiscalService
    {
        Task Inserir(List<MovimentoFiscal> movimentosFiscais);
        Task<List<MovimentoFiscalDTO>> ListarMovimentosFiscaisPendenteTransmissaoMercoCamp();
        Task Sincronizar();
        Task Atualizar(List<MovimentoFiscal> movimentosFiscais);
    }
}
