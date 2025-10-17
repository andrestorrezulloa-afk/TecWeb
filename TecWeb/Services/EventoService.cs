// Services/EventoService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TecWeb.DTOs;
using TecWeb.Models;

namespace TecWeb.Services
{
    public class EventoService : IEventoService
    {
        private readonly GestionCulturalContext _context;
        private readonly IMapper _mapper;

        public EventoService(GestionCulturalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<EventoDto>> CrearEventoAsync(EventoDto eventoDto)
        {
            if (eventoDto == null)
                return ServiceResult<EventoDto>.Failure("Evento nulo.");

            // Validar que el usuario asignado exista (regla de BD -> asincrónica)
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioId == eventoDto.UsuarioId);
            if (!usuarioExiste)
                return ServiceResult<EventoDto>.Failure("Usuario asignado no existe.");

            // (Opcional) Validación de conflicto de horarios, lógica de negocio, etc.
            // Ejemplo: evitar que el mismo usuario cree dos eventos en la misma fecha y lugar:
            var conflicto = await _context.Eventos.AnyAsync(e =>
                e.UsuarioId == eventoDto.UsuarioId &&
                e.Fecha == eventoDto.Fecha &&
                e.Lugar == eventoDto.Lugar);
            if (conflicto)
                return ServiceResult<EventoDto>.Failure("Ya existe un evento del mismo usuario en esa fecha y lugar.");

            var evento = _mapper.Map<Evento>(eventoDto);
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return ServiceResult<EventoDto>.Success(_mapper.Map<EventoDto>(evento), "Evento creado");
        }

        public async Task<ServiceResult<EventoDto>> ActualizarEventoAsync(int id, EventoDto eventoDto)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return ServiceResult<EventoDto>.Failure("Evento no encontrado.");

            // Validar usuario asignado existe
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.UsuarioId == eventoDto.UsuarioId);
            if (!usuarioExiste)
                return ServiceResult<EventoDto>.Failure("Usuario asignado no existe.");

            // (Opcional) Evitar conflicto con otra entidad existente
            var dup = await _context.Eventos.AnyAsync(e =>
                e.EventoId != id &&
                e.UsuarioId == eventoDto.UsuarioId &&
                e.Fecha == eventoDto.Fecha &&
                e.Lugar == eventoDto.Lugar);
            if (dup)
                return ServiceResult<EventoDto>.Failure("Existe otro evento del mismo usuario en esa fecha y lugar.");

            // Mapear campos permitidos
            evento.Titulo = eventoDto.Titulo;
            evento.Descripcion = eventoDto.Descripcion;
            evento.Lugar = eventoDto.Lugar;
            evento.Fecha = eventoDto.Fecha;
            evento.HoraInicio = eventoDto.HoraInicio;
            evento.HoraFin = eventoDto.HoraFin;
            evento.AforoMaximo = eventoDto.AforoMaximo;
            evento.UsuarioId = eventoDto.UsuarioId;

            await _context.SaveChangesAsync();

            return ServiceResult<EventoDto>.Success(_mapper.Map<EventoDto>(evento), "Evento actualizado");
        }

        public async Task<ServiceResult<bool>> EliminarEventoAsync(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return ServiceResult<bool>.Failure("Evento no encontrado.");

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true, "Evento eliminado");
        }

        public async Task<ServiceResult<List<EventoDto>>> ListarEventosAsync()
        {
            var eventos = await _context.Eventos
                .Include(e => e.Usuario) // opcional, si quieres info del usuario
                .ToListAsync();

            return ServiceResult<List<EventoDto>>.Success(_mapper.Map<List<EventoDto>>(eventos));
        }

        public async Task<ServiceResult<EventoDto>> ObtenerEventoPorIdAsync(int id)
        {
            var evento = await _context.Eventos
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.EventoId == id);

            if (evento == null)
                return ServiceResult<EventoDto>.Failure("Evento no encontrado.");

            return ServiceResult<EventoDto>.Success(_mapper.Map<EventoDto>(evento));
        }
    }
}