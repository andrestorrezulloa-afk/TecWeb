using AutoMapper;
using TecWeb.Core.DTOs;
using TecWeb.Core.Entities;

namespace TecWeb.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Inscripcione, InscripcionDto>().ReverseMap();
        }
    }
}
