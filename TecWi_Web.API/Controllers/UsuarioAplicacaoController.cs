using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;

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
        [Route(nameof(InsertAsync))]
        public async Task<IActionResult> InsertAsync(UsuarioAplicacaoDTO usuarioAplicacaoDTO)
        {
            ServiceResponse<Guid> serviceResponse = await _iUsuarioAplicacaoService.InsertAsync(usuarioAplicacaoDTO);
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
        [Route(nameof(BulkInsertAsync))]
        public async Task<IActionResult> BulkInsertAsync(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO)
        {
            ServiceResponse<bool> serviceResponse = await _iUsuarioAplicacaoService.BulkInsertAsync(usuarioAplicacaoDTO);
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
        [Route(nameof(DeleteAsync))]
        public async Task<IActionResult> DeleteAsync(Guid idUsuario)
        {
            ServiceResponse<bool> serviceResponse = await _iUsuarioAplicacaoService.DeleteAsync(idUsuario);
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
        public async Task<IActionResult> GetByIdUsuarioAsync(Guid idUsuario)
        {
            ServiceResponse<List<UsuarioAplicacaoDTO>> serviceResponse = await _iUsuarioAplicacaoService.GetByIdUsuarioAsync(idUsuario);
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
