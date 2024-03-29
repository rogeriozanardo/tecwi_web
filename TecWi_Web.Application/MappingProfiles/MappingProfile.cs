﻿using AutoMapper;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Entity to DTO
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.UsuarioAplicacaoDTO, opt => opt.MapFrom(src => src.UsuarioAplicacao));

            CreateMap<UsuarioAplicacao, UsuarioAplicacaoDTO>().ReverseMap();

            CreateMap<PagarReceber, PagarReceberDTO>().ReverseMap();

            CreateMap<LogOperacao, LogOperacaoDTO>().
                ForMember(dest => dest.UsuarioDTO, opt => opt.MapFrom(src => src.Usuario));

            CreateMap<Cliente, ClienteDTO>()
                .ForMember(dest => dest.ContatoCobrancaDTO, opt => opt.MapFrom(src => src.ContatoCobranca))
                .ForMember(dest => dest.PagarReceberDTO, opt => opt.MapFrom(src => src.PagarReceber))
                .ForMember(dest => dest.UsuarioDTO, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.ClienteContatoDTO, opt => opt.MapFrom(src => src.ClienteContato));

            CreateMap<ClienteContato, ClienteContatoDTO>().ReverseMap();

            CreateMap<ContatoCobranca, ContatoCobrancaDTO>()
                .ForMember(dest => dest.UsuarioDTO, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.ContatoCobrancaLancamentoDTO, opt => opt.MapFrom(src => src.ContatoCobrancaLancamento));

            CreateMap<ContatoCobrancaLancamento, ContatoCobrancaLancamentoDTO>();
            #endregion

            #region DTO to Entity
            CreateMap<UsuarioDTO, Usuario>();
            CreateMap<UsuarioAplicacaoDTO, UsuarioAplicacao>();
            CreateMap<ClienteDTO, Cliente>();
            CreateMap<ContatoCobrancaDTO, ContatoCobranca>();
            CreateMap<ContatoCobrancaLancamentoDTO, ContatoCobrancaLancamento>();
            #endregion
        }
    }
}
