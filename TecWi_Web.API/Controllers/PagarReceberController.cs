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

        public PagarReceberController(IPagarReceberService iPagarReceberService)
        {
            _iPagarReceberService = iPagarReceberService;
        }

        [HttpGet]
        [Route(nameof(PopulateData))]
        public async Task<IActionResult> PopulateData()
        {
            await Task.Delay(1);
            ServiceResponse<List<PagarReceberDTO>> serviceResponse = _iPagarReceberService.GetAllDapper();
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
