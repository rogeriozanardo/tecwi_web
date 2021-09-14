using AutoMapper;
using TecWi_Web.Domain.Entities;
using TecWi_Web.Shared.DTOs;

namespace TecWi_Web.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<UsuarioAplicacao, UsuarioAplicacaoDTO>().ReverseMap();
            CreateMap<PagarReceber, PagarReceberDTO>().ReverseMap();
            CreateMap<Cliente, ClienteDTO>()
                .ForMember(dest => dest.ContatoCobrancaDTO, opt => opt.MapFrom(src => src.ContatoCobranca))
                .ForMember(dest => dest.PagarReceberDTO, opt => opt.MapFrom(src => src.PagarReceber));
            CreateMap<ContatoCobranca, ContatoCobrancaDTO>()
                .ForMember(dest => dest.ContatoCobrancaLancamentoDTO, opt => opt.MapFrom(src => src.ContatoCobrancaLancamento));
            CreateMap<ContatoCobrancaLancamento, ContatoCobrancaLancamentoDTO>().ReverseMap();

            CreateMap<ClienteDTO, Cliente>();
            CreateMap<ContatoCobrancaDTO, ContatoCobranca>();
            CreateMap<ContatoCobrancaLancamentoDTO, ContatoCobrancaLancamento>();
        }
    }
}
