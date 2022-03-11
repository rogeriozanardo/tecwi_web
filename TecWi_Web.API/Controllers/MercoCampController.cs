using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Domain.Exceptions;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MercoCampController : ControllerBase
    {
        private readonly IPedidoMercoCampService _PedidoMercoCampService;
        private readonly ILogger<MercoCampController> _Logger;

        public MercoCampController(IPedidoMercoCampService pedidoMercoCampService,
                                   ILogger<MercoCampController> logger)
        {
            _PedidoMercoCampService = pedidoMercoCampService;
            _Logger = logger;
        }

        [HttpPost]
        [Route("enviar-separacao-pedido")]
        public async Task<IActionResult> ConfirmarSeparacaoPedido(CORPEM_WMS_CONF_SEP confirmacaoPedidoDTO)
        {
            try
            {
                await _PedidoMercoCampService.AtualizarStatusPedidosMercoCamp(confirmacaoPedidoDTO);
                return Ok("teste");
            }
            catch(PedidoNaoEncontradoException ex)
            {
                _Logger.LogError(ex?.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex?.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
