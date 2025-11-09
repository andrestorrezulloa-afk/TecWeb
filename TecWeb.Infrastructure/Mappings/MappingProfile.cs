using AutoMapper;
using TecWeb.Core.Entities;
using TecWeb.Infrastructure.DTOs;
using System;

namespace TecWeb.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<Usuario, UsuarioDto>().ReverseMap()
                .ForMember(dest => dest.Eventos, opt => opt.Ignore())       // Ignorar colecciones para evitar referencias circulares
                .ForMember(dest => dest.Inscripciones, opt => opt.Ignore())
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro ?? DateTime.UtcNow));

          
            CreateMap<Evento, EventoDto>().ReverseMap()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())        // Ignorar navegación
                .ForMember(dest => dest.Inscripciones, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.Lugar, opt => opt.MapFrom(src => src.Lugar))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo));

           
            CreateMap<Inscripcione, InscripcionDto>().ReverseMap()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())       // Ignorar navegación
                .ForMember(dest => dest.Evento, opt => opt.Ignore())
                .ForMember(dest => dest.Asistencia, opt => opt.MapFrom(src => src.Asistencia))
                .ForMember(dest => dest.FechaInscripcion, opt => opt.MapFrom(src => src.FechaInscripcion ?? DateTime.UtcNow));

          
        }
    }
}