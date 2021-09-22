﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.FrontServices.Interfaces
{
    public interface IUsuarioFrontService
    {
        Task<ServiceResponse<UsuarioDTO>> Login(UsuarioDTO usuarioDTO);
        Task<ServiceResponse<bool>> SalvarUsuario(UsuarioDTO usuarioDTO);
    }
}