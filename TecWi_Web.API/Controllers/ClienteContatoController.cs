using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteContatoController : ControllerBase
    {
        private readonly IClienteContatoService _IClienteContatoService;

        public ClienteContatoController(IClienteContatoService iClienteContatoService)
        {
            _IClienteContatoService = iClienteContatoService;
        }

        [HttpPost]
        [Route(nameof(InsertAsync))]
        public async Task<IActionResult> InsertAsync(ClienteContatoDTO clienteContatoDTO)
        {
            ServiceResponse<Guid> serviceResponse = await _IClienteContatoService.InsertAsync(clienteContatoDTO);

            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse);
            }
        }

        [HttpPut]
        [Route(nameof(UpdateAsync))]
        public async Task<IActionResult> UpdateAsync([FromBody] ClienteContatoDTO clienteContatoDTO)
        {
            ServiceResponse<Guid> serviceResponse = await _IClienteContatoService.UpdateAsync(clienteContatoDTO);

            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse);
            }
        }

        [HttpGet]
        [Route(nameof(GetByClienteAsync))]
        public async Task<IActionResult> GetByClienteAsync(ClienteContatoFilter clienteContatoFilter)
        {
            ServiceResponse<List<ClienteContatoDTO>> serviceResponse = await _IClienteContatoService.GetByClienteAsync(clienteContatoFilter.Cdclifor);

            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse);
            }
        }


        [HttpDelete]
        [Route(nameof(DeleteAsync))]
        public async Task<IActionResult> DeleteAsync([FromBody] ClienteContatoFilter clienteContatoFilter)
        {
            ServiceResponse<bool> serviceResponse = await _IClienteContatoService.DeleteAsync(clienteContatoFilter.IdClienteContato);

            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse);
            }
        }
    }
}
