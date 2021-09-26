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
    public class ContatoCobrancaController : ControllerBase
    {
        private readonly IContatoCobrancaService _iContatoCobrancaService;

        public ContatoCobrancaController(IContatoCobrancaService iContatoCobrancaService)
        {
            _iContatoCobrancaService = iContatoCobrancaService;
        }

        [HttpPost]
        [Route(nameof(GetAllAsync))]
        public async Task<IActionResult> GetAllAsync(ContatoCobrancaFilter contatoCobrancaFilter)
        {
            ServiceResponse<List<ContatoCobrancaDTO>> serviceResponse = await _iContatoCobrancaService.GetAllAsync(contatoCobrancaFilter);

            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse.Message);
            }
        }

        [HttpPost]
        [Route(nameof(InsertAsync))]
        public async Task<IActionResult> InsertAsync(ContatoCobrancaDTO contatoCobrancaDTO)
        {
            ServiceResponse<bool> serviceResponse = await _iContatoCobrancaService.InsertAsync(contatoCobrancaDTO);

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
