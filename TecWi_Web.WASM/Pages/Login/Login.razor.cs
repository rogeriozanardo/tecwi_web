
using System;
using System.Threading.Tasks;
using TecWi_Web.FrontServices;
using TecWi_Web.FrontServices.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.Login
{
    
    partial class Login
    {
        

        bool isChecked = false;
        private UsuarioDTO usuarioDTO = new UsuarioDTO();
        private string ano = DateTime.Now.Year.ToString();

    
        private async Task FazLogin()
        {
            ServiceResponse<UsuarioDTO> serviceResponse = new ServiceResponse<UsuarioDTO>();

            serviceResponse = await usuarioFrontService.Login(usuarioDTO);
            if(!serviceResponse.Success)
            {

            }else if(Config.Autorizado)
            {
                Navigation.NavigateTo("/Home");
            }
        }

        
    }
}
