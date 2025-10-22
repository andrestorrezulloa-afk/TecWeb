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
        private readonly IInscripcionRepository _repo;
        private readonly IEventoRepository _eventoRepo;
        private readonly IMapper _mapper;

        public InscripcionService(IInscripcionRepository repo, IEventoRepository eventoRepo, IMapper mapper)
        {
            _repo = repo;
            _eventoRepo = eventoRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResult<InscripcionDto>> CrearInscripcionAsync(InscripcionDto inscripcionDto)
        {
            if (inscripcionDto == null) return ServiceResult<InscripcionDto>.Failure("Inscripción nula");

            // Validaciones básicas: usuario no esté ya inscripto, que evento exista y no supere aforo
            if (await _repo.UsuarioInscriptoEnEventoAsync(inscripcionDto.UsuarioId, inscripcionDto.EventoId))
                return ServiceResult<InscripcionDto>.Failure("Usuario ya inscrito en el evento");

            var evento = await _eventoRepo.ObtenerPorIdAsync(inscripcionDto.EventoId);
            if (evento == null) return ServiceResult<InscripcionDto>.Failure("Evento no encontrado");

            var cantidadInscritos = evento.Inscripciones?.Count ?? 0;
            if (cantidadInscritos >= evento.AforoMaximo)
                return ServiceResult<InscripcionDto>.Failure("Evento lleno");

            var entidad = _mapper.Map<Inscripcione>(inscripcionDto);
            var creado = await _repo.CrearAsync(entidad);
            return ServiceResult<InscripcionDto>.Success(_mapper.Map<InscripcionDto>(creado), "Inscripción creada");
        }

        public async Task<ServiceResult<bool>> EliminarInscripcionAsync(int id)
        {
            var ins = await _repo.ObtenerPorIdAsync(id);
            if (ins == null) return ServiceResult<bool>.Failure("Inscripción no encontrada");
            await _repo.EliminarAsync(ins);
            return ServiceResult<bool>.Success(true, "Inscripción eliminada");
        }

        public async Task<ServiceResult<List<InscripcionDto>>> ListarInscripcionesPorEventoAsync(int eventoId)
        {
            var list = await _repo.ListarPorEventoAsync(eventoId);
            return ServiceResult<List<InscripcionDto>>.Success(_mapper.Map<List<InscripcionDto>>(list));
        }

        public async Task<ServiceResult<InscripcionDto>> ObtenerInscripcionPorIdAsync(int id)
        {
            var ins = await _repo.ObtenerPorIdAsync(id);
            if (ins == null) return ServiceResult<InscripcionDto>.Failure("Inscripción no encontrada");
            return ServiceResult<InscripcionDto>.Success(_mapper.Map<InscripcionDto>(ins));
        }
    }
}
