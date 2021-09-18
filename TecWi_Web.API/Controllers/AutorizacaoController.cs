using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizacaoController : ControllerBase
    {
        private readonly IAutorizacaoService _iAutorizacaoService;

        public AutorizacaoController(IAutorizacaoService iAutorizacaoService)
        {
            _iAutorizacaoService = iAutorizacaoService;
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(UsuarioDTO UsuarioDTO)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = await _iAutorizacaoService.Register(UsuarioDTO);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse.Message);
            }

            return Ok(serviceResponse);
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult> Login(UsuarioDTO UsuarioDTO)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = await _iAutorizacaoService.Login(UsuarioDTO);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }

            return Ok(serviceResponse.Data);
        }
    }
}
