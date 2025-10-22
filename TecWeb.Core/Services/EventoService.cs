using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TecWeb.Core.DTOs;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;

namespace TecWeb.Core.Services
{
    public class EventoService : IEventoService
    {
        private readonly IEventoRepository _repo;
        private readonly IMapper _mapper;

        public EventoService(IEventoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ServiceResult<EventoDto>> CrearEventoAsync(EventoDto eventoDto)
        {
            if (eventoDto == null) return ServiceResult<EventoDto>.Failure("Evento nulo");

            
            if (!await _repo.UsuarioExisteAsync(eventoDto.UsuarioId))
                return ServiceResult<EventoDto>.Failure("Usuario no existe");

            
            if (await _repo.ExisteConflictoAsync(eventoDto.UsuarioId, eventoDto.Fecha, eventoDto.Lugar))
                return ServiceResult<EventoDto>.Failure("Conflicto: ya existe un evento del usuario en esa fecha/lugar");

            var entidad = _mapper.Map<Evento>(eventoDto);
            var creado = await _repo.CrearAsync(entidad);

            return ServiceResult<EventoDto>.Success(_mapper.Map<EventoDto>(creado), "Evento creado");
        }

        public async Task<ServiceResult<bool>> EliminarEventoAsync(int id)
        {
            var e = await _repo.ObtenerPorIdAsync(id);
            if (e == null) return ServiceResult<bool>.Failure("Evento no encontrado");

            await _repo.EliminarAsync(e);
            return ServiceResult<bool>.Success(true, "Evento eliminado");
        }

        public async Task<ServiceResult<EventoDto>> ObtenerEventoPorIdAsync(int id)
        {
            var e = await _repo.ObtenerPorIdAsync(id);
            if (e == null) return ServiceResult<EventoDto>.Failure("Evento no encontrado");

            return ServiceResult<EventoDto>.Success(_mapper.Map<EventoDto>(e));
        }

        public async Task<ServiceResult<List<EventoDto>>> ListarEventosAsync()
        {
            var list = await _repo.ListarAsync();
            return ServiceResult<List<EventoDto>>.Success(_mapper.Map<List<EventoDto>>(list));
        }

        public async Task<ServiceResult<EventoDto>> ActualizarEventoAsync(int id, EventoDto eventoDto)
        {
            var e = await _repo.ObtenerPorIdAsync(id);
            if (e == null) return ServiceResult<EventoDto>.Failure("Evento no encontrado");

            // Mapear los cambios desde el DTO a la entidad existente
            _mapper.Map(eventoDto, e);
            await _repo.ActualizarAsync(e);

            return ServiceResult<EventoDto>.Success(_mapper.Map<EventoDto>(e), "Evento actualizado");
        }
    }
}
