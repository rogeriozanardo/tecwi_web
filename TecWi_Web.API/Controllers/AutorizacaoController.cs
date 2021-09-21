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
        [Route(nameof(LoginAsync))]
        public async Task<ActionResult> LoginAsync(UsuarioDTO UsuarioDTO)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = await _iAutorizacaoService.LoginAsync(UsuarioDTO);
            if (!serviceResponse.Success)
            {
                return BadRequest(serviceResponse);
            }

            return Ok(serviceResponse.Data);
        }
    }
}
