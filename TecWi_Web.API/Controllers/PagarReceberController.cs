using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Domain.Enums;
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
        private readonly ILogOperacaoService _iLogOperacaoService;

        public PagarReceberController(IPagarReceberService iPagarReceberService, IClienteService iClienteService, ILogOperacaoService logOperacaoService)
        {
            _iPagarReceberService = iPagarReceberService;
            _iClienteService = iClienteService;
            _iLogOperacaoService = logOperacaoService;
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

        [HttpPost]
        [Route(nameof(GetNewestAsync))]
        public async Task<IActionResult> GetNewestAsync()
        {
            ServiceResponse<LogOperacaoDTO> serviceResponseUsuario = await _iLogOperacaoService.GetNewestAsync(TipoOperacao.AtualizarDadosCobranca);

            ServiceResponse<DateTime> serviceResponse = new ServiceResponse<DateTime>();

            serviceResponse.Data = serviceResponseUsuario.Data.Data;

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
