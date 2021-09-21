using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PagarReceberController : ControllerBase
    {
        private readonly IPagarReceberService _iPagarReceberService;
        private readonly IClienteService _iClienteService;

        public PagarReceberController(IPagarReceberService iPagarReceberService, IClienteService iClienteService)
        {
            _iPagarReceberService = iPagarReceberService;
            _iClienteService = iClienteService;
        }

        [HttpGet]
        [Route(nameof(PopulateDataAsync))]
        public async Task<IActionResult> PopulateDataAsync()
        {
            ServiceResponse<DateTime> serviceResponse =  await _iPagarReceberService.PopulateDataAsync();

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
