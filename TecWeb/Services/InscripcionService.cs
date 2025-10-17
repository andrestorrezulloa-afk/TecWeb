// Services/InscripcionService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TecWeb.DTOs;
using TecWeb.Models;

namespace TecWeb.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly GestionCulturalContext _context;
        private readonly IMapper _mapper;

        public InscripcionService(GestionCulturalContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResult<InscripcioneDto>> CrearInscripcionAsync(InscripcioneDto inscripcionDto)
        {
            if (inscripcionDto == null)
                return ServiceResult<InscripcioneDto>.Failure("Inscripción nula");

            if (!await _context.Usuarios.AnyAsync(u => u.UsuarioId == inscripcionDto.UsuarioId))
                return ServiceResult<InscripcioneDto>.Failure("Usuario no existe");

            if (!await _context.Eventos.AnyAsync(e => e.EventoId == inscripcionDto.EventoId))
                return ServiceResult<InscripcioneDto>.Failure("Evento no existe");

            if (await _context.Inscripciones.AnyAsync(i => i.UsuarioId == inscripcionDto.UsuarioId && i.EventoId == inscripcionDto.EventoId))
                return ServiceResult<InscripcioneDto>.Failure("Usuario ya inscrito en este evento");

            var inscripcion = _mapper.Map<Inscripcione>(inscripcionDto);
            inscripcion.FechaInscripcion = inscripcion.FechaInscripcion ?? DateTime.Now;

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            return ServiceResult<InscripcioneDto>.Success(_mapper.Map<InscripcioneDto>(inscripcion), "Inscripción creada");
        }

        public async Task<ServiceResult<InscripcioneDto>> ActualizarInscripcionAsync(int id, InscripcioneDto inscripcionDto)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
                return ServiceResult<InscripcioneDto>.Failure("Inscripción no encontrada");

            if (!await _context.Usuarios.AnyAsync(u => u.UsuarioId == inscripcionDto.UsuarioId))
                return ServiceResult<InscripcioneDto>.Failure("Usuario no existe");

            if (!await _context.Eventos.AnyAsync(e => e.EventoId == inscripcionDto.EventoId))
                return ServiceResult<InscripcioneDto>.Failure("Evento no existe");

            // Evitar duplicados al actualizar: no permitir que cambie a una combinación ya existente
            var dup = await _context.Inscripciones
                .AnyAsync(i => i.InscripcionId != id && i.UsuarioId == inscripcionDto.UsuarioId && i.EventoId == inscripcionDto.EventoId);
            if (dup)
                return ServiceResult<InscripcioneDto>.Failure("Ya existe otra inscripción para ese usuario y evento.");

            inscripcion.UsuarioId = inscripcionDto.UsuarioId;
            inscripcion.EventoId = inscripcionDto.EventoId;
            inscripcion.Asistencia = inscripcionDto.Asistencia;
            inscripcion.FechaInscripcion = inscripcionDto.FechaInscripcion ?? inscripcion.FechaInscripcion ?? DateTime.Now;

            await _context.SaveChangesAsync();

            return ServiceResult<InscripcioneDto>.Success(_mapper.Map<InscripcioneDto>(inscripcion), "Inscripción actualizada");
        }

        public async Task<ServiceResult<bool>> EliminarInscripcionAsync(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
                return ServiceResult<bool>.Failure("Inscripción no encontrada");

            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();
            return ServiceResult<bool>.Success(true, "Inscripción eliminada");
        }

        public async Task<ServiceResult<List<InscripcioneDto>>> ListarInscripcionesAsync()
        {
            var inscripciones = await _context.Inscripciones
                .Include(i => i.Usuario)
                .Include(i => i.Evento)
                .ToListAsync();

            var dtos = _mapper.Map<List<InscripcioneDto>>(inscripciones);
            return ServiceResult<List<InscripcioneDto>>.Success(dtos);
        }

        public async Task<ServiceResult<InscripcioneDto>> ObtenerInscripcionPorIdAsync(int id)
        {
            var inscripcion = await _context.Inscripciones
                .Include(i => i.Usuario)
                .Include(i => i.Evento)
                .FirstOrDefaultAsync(i => i.InscripcionId == id);

            if (inscripcion == null)
                return ServiceResult<InscripcioneDto>.Failure("Inscripción no encontrada");

            return ServiceResult<InscripcioneDto>.Success(_mapper.Map<InscripcioneDto>(inscripcion));
        }
    }
}