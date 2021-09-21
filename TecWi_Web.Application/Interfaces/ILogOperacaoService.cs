using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.Application.Interfaces
{
    public interface ILogOperacaoService
    {
        Task<ServiceResponse<List<LogOperacaoDTO>>> GetAllAsync(LogOperacaoFilter logOperacaoFilter);
        Task<ServiceResponse<LogOperacaoDTO>> GetNewestAsync(TipoOperacao tipoOperacao);
        Task<ServiceResponse<bool>> ClearLogOperacaoAsync(int OlderThanInDays);
        Task<ServiceResponse<Guid>> InsertAsync(LogOperacaoDTO LogOperacaoDTO);
    }
}
