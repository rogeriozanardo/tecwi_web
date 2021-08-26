using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Application.Interfaces;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.Services
{
    public class AutorizacaoService : IAutorizacaoService
    {
        public Task<ServiceResponse<string>> Login(UsuarioDTO usuarioDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<UsuarioDTO>> Registrar(UsuarioDTO usuarioDTO)
        {
            throw new NotImplementedException();
        }
    }
}
