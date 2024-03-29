﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWi_Web.Shared.DTOs;
using TecWi_Web.Shared.Filters;

namespace TecWi_Web.FrontServices.Interfaces
{
    public interface IUsuarioFrontService
    {
        Task<ServiceResponse<UsuarioDTO>> Login(UsuarioDTO usuarioDTO);
        Task<ServiceResponse<bool>> SalvarUsuario(UsuarioDTO usuarioDTO);
        Task<ServiceResponse<List<UsuarioDTO>>> GetAllAsync(UsuarioFilter usuarioFilter);
        Task<ServiceResponse<bool>> UpdateJustInfoAsync(UsuarioDTO usuarioDTO);

        Task<ServiceResponse<bool>> AtualizaAplicacoesUsuario(List<UsuarioAplicacaoDTO> usuarioAplicacaoDTO);
    }
}
