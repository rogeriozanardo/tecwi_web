
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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _iUsuarioService;

        public UsuarioController(IUsuarioService iUsuarioService)
        {
            _iUsuarioService = iUsuarioService;
        }


        [HttpPost]
        [Route(nameof(GetAllAsync))]
        public async Task<IActionResult> GetAllAsync(UsuarioFilter usuarioFilter)
        {
            ServiceResponse<List<UsuarioDTO>> serviceResponse = await _iUsuarioService.GetAllAsync(usuarioFilter);
            
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
        [Route(nameof(GetByLoginAsync))]
        public async Task<IActionResult> GetByLoginAsync(UsuarioFilter usuarioFilter)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = await _iUsuarioService.GetByLoginAsync(usuarioFilter.Login);

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
        [Route(nameof(GetByIdAsync))]
        public async Task<IActionResult> GetByIdAsync(UsuarioFilter usuarioFilter)
        {
            ServiceResponse<UsuarioDTO> serviceResponse = await _iUsuarioService.GetByIdAsync(usuarioFilter.IdUsuario);

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
        [Route(nameof(InsertAsync))]
        public async Task<IActionResult> InsertAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = await _iUsuarioService.InsertAsync(usuarioDTO);

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
        [Route(nameof(UpdateAsync))]
        public async Task<IActionResult> UpdateAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = await _iUsuarioService.UpdateAsync(usuarioDTO);

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
        [Route(nameof(UpdateJustInfoAsync))]
        public async Task<IActionResult> UpdateJustInfoAsync(UsuarioDTO usuarioDTO)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();

            serviceResponse = await _iUsuarioService.UpdateJustInfoAsync(usuarioDTO);

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
