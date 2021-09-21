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
    public class UsuarioAplicacaoController : ControllerBase
    {
        private readonly IUsuarioAplicacaoService _iUsuarioAplicacaoService;
        public UsuarioAplicacaoController(IUsuarioAplicacaoService iUsuarioAplicacaoService)
        {
            _iUsuarioAplicacaoService = iUsuarioAplicacaoService;
        }

        [HttpPost]
        [Route(nameof(Insert))]
        public async Task<IActionResult> Insert(UsuarioAplicacaoDTO usuarioAplicacaoDTO)
        {
            ServiceResponse<Guid> serviceResponse = await _iUsuarioAplicacaoService.Insert(usuarioAplicacaoDTO);
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
        [Route(nameof(BulkInsert))]
        public async Task<IActionResult> BulkInsert(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO)
        {
            ServiceResponse<bool> serviceResponse = await _iUsuarioAplicacaoService.BulkInsert(usuarioAplicacaoDTO);
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse.Message);
            }
        }

        [HttpDelete]
        [Route(nameof(Delete))]
        public async Task<IActionResult> Delete(Guid idUsuario)
        {
            ServiceResponse<bool> serviceResponse = await _iUsuarioAplicacaoService.Delete(idUsuario);
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }
            else
            {
                return BadRequest(serviceResponse.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByIdUsuario(Guid idUsuario)
        {
            ServiceResponse<List<UsuarioAplicacaoDTO>> serviceResponse = await _iUsuarioAplicacaoService.GetByIdUsuario(idUsuario);
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
        [Route(nameof(GetUserList))]
        public async Task<IActionResult> GetUserList(UsuarioFilter usuarioFilter)
        {
            ServiceResponse<List<UsuarioDTO>> serviceResponse = new ServiceResponse<List<UsuarioDTO>>();

            serviceResponse.Data = new List<UsuarioDTO>();
            //serviceResponse = await _iUsuarioAplicacaoService
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
