using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Interfaces
{
    public interface IClienteContatoService
    {
        Task<ServiceResponse<Guid>> InsertAsync(ClienteContatoDTO clienteContatoDTO);
        Task<ServiceResponse<bool>> UpdateAsync(ClienteContatoDTO clienteContatoDTO);
        Task<ServiceResponse<List<ClienteContatoDTO>>> GetByClienteAsync (int Cdclifor);
        Task<ServiceResponse<bool>> DeleteAsync(Guid IdClienteContato);
    }
}
