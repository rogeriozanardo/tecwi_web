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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _iUsuarioService;

        public UsuarioController(IUsuarioService iUsuarioService)
        {
            _iUsuarioService = iUsuarioService;
        }


        [HttpPost]
        [Route(nameof(GetAllAsycn))]
        public async Task<IActionResult> GetAllAsycn(UsuarioFilter usuarioFilter)
        {
            ServiceResponse<List<UsuarioDTO>> serviceResponse = await _iUsuarioService.GetAllAsycn(usuarioFilter);
            
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
        [Route(nameof(Update))]
        public async Task<IActionResult> Update(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = await _iUsuarioService.Update(usuarioDTO);

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
