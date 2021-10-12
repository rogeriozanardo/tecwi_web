using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.FrontServices.Interfaces
{
    public interface IClienteFrontservice
    {
        Task<ServiceResponse<ClienteDTO>> BuscaProximoClienteFilaPorUsuario();
        Task<ServiceResponse<List<ClienteDTO>>> PesquisaCliente(string pesquisa);
        Task<ServiceResponse<bool>> SalvaCliente(ClienteDTO clienteDTO);
        Task<ServiceResponse<Guid>> SalvaContatoCliente(ClienteContatoDTO clienteContatoDTO);
    }
}
