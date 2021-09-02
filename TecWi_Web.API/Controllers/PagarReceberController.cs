using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Route(nameof(PopulateData))]
        public async Task<IActionResult> PopulateData()
        {
            ServiceResponse<bool> serviceResponse =  await _iPagarReceberService.PopulateData();

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
