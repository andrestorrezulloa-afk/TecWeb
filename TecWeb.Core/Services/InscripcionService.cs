using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TecWeb.Core.DTOs;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Core.Services;

namespace TecWeb.Core.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly IInscripcionRepository _insRepo;
        private readonly IEventoRepository _eventoRepo;
        private readonly IMapper _mapper;

        public InscripcionService(IInscripcionRepository insRepo, IEventoRepository eventoRepo, IMapper mapper)
        {
            _insRepo = insRepo;
            _eventoRepo = eventoRepo;
            _mapper = mapper;
        }

        // Método faltante que estaba causando CS0535
        public async Task<ServiceResult<List<InscripcionDto>>> ListarInscripcionesAsync()
        {
            var list = await _insRepo.ListarAsync();
            var mapped = _mapper.Map<List<InscripcionDto>>(list);
            return ServiceResult<List<InscripcionDto>>.Success(mapped);
        }

        public async Task<ServiceResult<List<InscripcionDto>>> ListarInscripcionesPorEventoAsync(int eventoId)
        {
            var list = await _insRepo.ListarPorEventoAsync(eventoId);
            var mapped = _mapper.Map<List<InscripcionDto>>(list);
            return ServiceResult<List<InscripcionDto>>.Success(mapped);
        }

        public async Task<ServiceResult<InscripcionDto>> ObtenerInscripcionPorIdAsync(int id)
        {
            var ins = await _insRepo.ObtenerPorIdAsync(id);
            if (ins == null) return ServiceResult<InscripcionDto>.Failure("Inscripción no encontrada");
            return ServiceResult<InscripcionDto>.Success(_mapper.Map<InscripcionDto>(ins));
        }

        public async Task<ServiceResult<InscripcionDto>> CrearInscripcionAsync(InscripcionDto dto)
        {
            if (dto == null) return ServiceResult<InscripcionDto>.Failure("Inscripción nula");

            if (await _insRepo.UsuarioInscriptoEnEventoAsync(dto.UsuarioId, dto.EventoId))
                return ServiceResult<InscripcionDto>.Failure("Usuario ya inscrito");

            var evento = await _eventoRepo.ObtenerPorIdAsync(dto.EventoId);
            if (evento == null) return ServiceResult<InscripcionDto>.Failure("Evento no encontrado");

            var cantidad = evento.Inscripciones?.Count ?? 0;
            if (cantidad >= evento.AforoMaximo) return ServiceResult<InscripcionDto>.Failure("Evento lleno");

            var entidad = _mapper.Map<Inscripcione>(dto);
            var creado = await _insRepo.CrearAsync(entidad);
            return ServiceResult<InscripcionDto>.Success(_mapper.Map<InscripcionDto>(creado), "Inscripción creada");
        }

        public async Task<ServiceResult<InscripcionDto>> ActualizarInscripcionAsync(int id, InscripcionDto dto)
        {
            var ins = await _insRepo.ObtenerPorIdAsync(id);
            if (ins == null) return ServiceResult<InscripcionDto>.Failure("Inscripción no encontrada");

            _mapper.Map(dto, ins);
            await _insRepo.ActualizarAsync(ins);
            return ServiceResult<InscripcionDto>.Success(_mapper.Map<InscripcionDto>(ins), "Inscripción actualizada");
        }

        public async Task<ServiceResult<bool>> EliminarInscripcionAsync(int id)
        {
            var ins = await _insRepo.ObtenerPorIdAsync(id);
            if (ins == null) return ServiceResult<bool>.Failure("Inscripción no encontrada");
            await _insRepo.EliminarAsync(ins);
            return ServiceResult<bool>.Success(true, "Inscripción eliminada");
        }
    }
}
