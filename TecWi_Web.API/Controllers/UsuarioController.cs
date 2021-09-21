using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;
namespace TecWi_Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService iUsuarioService;
    }
}
