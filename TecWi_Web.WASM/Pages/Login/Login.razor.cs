
using System;
using System.Threading.Tasks;
using TecWi_Web.FrontServices;
using TecWi_Web.FrontServices.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.WASM.Pages.Login
{
    
    partial class Login
    {
        bool exibeSpinner = false;
        public MensagemInformativaDTO mensagemInformativaDTO = new MensagemInformativaDTO();
        bool isChecked = false;
        private UsuarioDTO usuarioDTO = new UsuarioDTO();
        private string ano = DateTime.Now.Year.ToString();

    
        private async Task FazLogin()
        {
            ServiceResponse<UsuarioDTO> serviceResponse = new ServiceResponse<UsuarioDTO>();

            exibeSpinner = true;
            serviceResponse = await usuarioFrontService.Login(usuarioDTO);
            if(!serviceResponse.Success)
            {
                exibeSpinner = false;
                mensagemInformativaDTO.Titulo = "Atenção";
                mensagemInformativaDTO.Mensagem = serviceResponse.Message;
                mensagemInformativaDTO.Exibe = true;

            }else if(Config.Autorizado)
            {
                exibeSpinner = false;
                Navigation.NavigateTo("/Home");
            }
        }

        
    }
}
