using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _iClienteService;

        public ClienteController(IClienteService iClienteService)
        {
            _iClienteService = iClienteService;
        }

        [HttpPost]
        [Route(nameof(GetAllAsync))]
        public async Task<IActionResult> GetAllAsync(ClientePagarReceberFilter clientePagarReceberFilterr)
        {
            ServiceResponse<List<ClienteDTO>> serviceResponse = await _iClienteService.GetAllAsync(clientePagarReceberFilterr);

            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse.Message);
            }
        }
    }
}
