using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Domain.Enums;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LogOperacaoController : ControllerBase
    {
        private readonly ILogOperacaoService _iLogOperacaoService;

        public LogOperacaoController(ILogOperacaoService iLogOperacaoService)
        {
            _iLogOperacaoService = iLogOperacaoService;
        }

        [HttpPost]
        [Route(nameof(GetAllAsync))]
        public async Task<IActionResult> GetAllAsync(LogOperacaoFilter logOperacaoFilter)
        {
            ServiceResponse<List<LogOperacaoDTO>> serviceResponse = await _iLogOperacaoService.GetAllAsync(logOperacaoFilter);

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
        [Route(nameof(GetNewestAsync))]
        public async Task<IActionResult> GetNewestAsync(TipoOperacao tipoOperacao)
        {
            ServiceResponse<LogOperacaoDTO> serviceResponse = await _iLogOperacaoService.GetNewestAsync(tipoOperacao);

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
