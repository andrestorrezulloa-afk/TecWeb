    using AutoMapper;
    using TecWeb.Models;
    using TecWeb.DTOs;

    namespace TecWeb.Mappings
    {
        public class MappingProfile : Profile
        {
            public MappingProfile() 
            {
                CreateMap<Evento, EventoDto>();
                CreateMap<EventoDto, Evento>();
                CreateMap<Usuario, UsuarioDto>();
                CreateMap<UsuarioDto, Usuario>();
                CreateMap<Inscripcione, InscripcioneDto>();
                CreateMap<InscripcioneDto, Inscripcione>();
        }
        }
    }
