using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces.JobsSincronizacao;
using TecWi_Web.Data.Repositories.Sincronizacao.Interfaces;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Services.JobsSincronizacao
{
    public class MovimentoFiscalService : IMovimentoFiscalService
    {
        private readonly IMovimentoFiscalRepository _MovimentoFiscalRepository;

        public MovimentoFiscalService(IMovimentoFiscalRepository movimentoFiscalRepository)
        {
            _MovimentoFiscalRepository = movimentoFiscalRepository;
        }

        public async Task Inserir(List<MovimentoFiscal> movimentosFiscais)
        {
            await _MovimentoFiscalRepository.Inserir(movimentosFiscais);
        }

        public async Task<List<MovimentoFiscalDTO>> ListarMovimentosFiscaisPendenteTransmissaoMercoCamp()
        {
           return await _MovimentoFiscalRepository.ListarMovimentosFiscaisPendenteTransmissaoMercoCamp();
        }

        public async Task Sincronizar()
        {
            await _MovimentoFiscalRepository.Sincronizar();
        }

        public async Task Atualizar(List<MovimentoFiscal> movimentosFiscais)
        {
            await _MovimentoFiscalRepository.Atualizar(movimentosFiscais);
        }
    }
}
