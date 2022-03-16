using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AtendenteController : ControllerBase
    {
        private readonly IAtendenteService _atendenteService;
        public AtendenteController(IAtendenteService atendenteService)
        {
            _atendenteService = atendenteService;
        }

        [HttpPost]
        [Route(nameof(ListaPerformanceAtendentes))]
        public async Task<IActionResult> ListaPerformanceAtendentes(PesquisaDTO  pesquisaDTO)
        {
            ServiceResponse<List<AtendenteDTO>> serviceResponse = await _atendenteService.ListaPerformanceAtendentes(pesquisaDTO);
            
            if(serviceResponse.Success)
            {
                return Ok(serviceResponse);
            }else
            {
                return BadRequest(serviceResponse);
            }
        }
    }
}
