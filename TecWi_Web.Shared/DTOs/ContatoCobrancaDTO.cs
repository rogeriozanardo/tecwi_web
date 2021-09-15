﻿using System;
using System.Collections.Generic;
using TecWi_Web.Domain.Enums;

namespace TecWi_Web.Shared.DTOs
{
    public class ContatoCobrancaDTO
    {
        public int IdContato { get; set; }
        public int Cdclifor { get; set; }
        public Guid IdUsuario { get; set; }
        public DateTime DtContato { get; set; }
        public string Anotacao { get; set; }
        public TipoContato TipoContato { get; set; }
        public DateTime DtAgenda { get; set; }
        public UsuarioDTO UsuarioDTO { get; set; }
        public ClienteDTO ClienteDTO { get; set; }
        public List<ContatoCobrancaLancamentoDTO> ContatoCobrancaLancamentoDTO { get; set; }
    }
}