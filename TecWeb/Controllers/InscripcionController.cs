using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWeb.Core.Entities;
using TecWeb.Core.Exceptions;
using TecWeb.Core.Interfaces;
using TecWeb.Core.QueryFilters;
using TecWeb.Core.CustomEntities; // <- Para PagedList

namespace TecWeb.Core.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InscripcionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Método con filtros y paginación
        public async Task<ServiceResult<PagedList<Inscripcione>>> ListarInscripcionesAsync(InscripcionQueryFilter filters = null)
        {
            var list = await _unitOfWork.InscripcionRepository.ListarAsync();
            var query = list.AsQueryable();

            if (filters != null)
            {
                if (filters.UsuarioId.HasValue)
                    query = query.Where(i => i.UsuarioId == filters.UsuarioId.Value);

                if (filters.EventoId.HasValue)
                    query = query.Where(i => i.EventoId == filters.EventoId.Value);

                if (filters.FechaInscripcion.HasValue)
                    query = query.Where(i => i.FechaInscripcion.HasValue &&
                                             i.FechaInscripcion.Value.Date == filters.FechaInscripcion.Value.Date);

                if (filters.Asistencia.HasValue)
                    query = query.Where(i => i.Asistencia == filters.Asistencia.Value);
            }

            // Aplicar paginación
            var pagedInscripciones = PagedList<Inscripcione>.Create(
                query,
                filters?.PageNumber ?? 1,
                filters?.PageSize ?? 10
            );

            return ServiceResult<PagedList<Inscripcione>>.Success(pagedInscripciones);
        }

        public async Task<ServiceResult<List<Inscripcione>>> ListarInscripcionesPorEventoAsync(int eventoId)
        {
            var list = await _unitOfWork.InscripcionRepository.ListarPorEventoAsync(eventoId);
            return ServiceResult<List<Inscripcione>>.Success(list);
        }

        public async Task<ServiceResult<Inscripcione>> ObtenerInscripcionPorIdAsync(int id)
        {
            var ins = await _unitOfWork.InscripcionRepository.ObtenerPorIdAsync(id);
            if (ins == null)
                throw new BusinessException("Inscripción no encontrada", 404);

            return ServiceResult<Inscripcione>.Success(ins);
        }

        public async Task<ServiceResult<Inscripcione>> CrearInscripcionAsync(Inscripcione inscripcion)
        {
            if (inscripcion == null)
                throw new BusinessException("Inscripción nula", 400);

            if (await _unitOfWork.InscripcionRepository.UsuarioInscriptoEnEventoAsync(inscripcion.UsuarioId, inscripcion.EventoId))
                throw new BusinessException("Usuario ya inscrito en este evento", 400);

            var evento = await _unitOfWork.EventoRepository.ObtenerPorIdAsync(inscripcion.EventoId);
            if (evento == null)
                throw new BusinessException("Evento no encontrado", 404);

            var cantidad = evento.Inscripciones?.Count ?? 0;
            if (cantidad >= evento.AforoMaximo)
                throw new BusinessException("Evento lleno", 400);

            await _unitOfWork.InscripcionRepository.CrearAsync(inscripcion);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<Inscripcione>.Success(inscripcion, "Inscripción creada exitosamente");
        }

        public async Task<ServiceResult<Inscripcione>> ActualizarInscripcionAsync(int id, Inscripcione inscripcion)
        {
            var ins = await _unitOfWork.InscripcionRepository.ObtenerPorIdAsync(id);
            if (ins == null)
                throw new BusinessException("Inscripción no encontrada", 404);

            ins.UsuarioId = inscripcion.UsuarioId;
            ins.EventoId = inscripcion.EventoId;
            ins.FechaInscripcion = inscripcion.FechaInscripcion;
            ins.Asistencia = inscripcion.Asistencia;

            _unitOfWork.InscripcionRepository.Actualizar(ins);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<Inscripcione>.Success(ins, "Inscripción actualizada correctamente");
        }

        public async Task<ServiceResult<bool>> EliminarInscripcionAsync(int id)
        {
            var ins = await _unitOfWork.InscripcionRepository.ObtenerPorIdAsync(id);
            if (ins == null)
                throw new BusinessException("Inscripción no encontrada", 404);

            _unitOfWork.InscripcionRepository.Eliminar(ins);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<bool>.Success(true, "Inscripción eliminada correctamente");
        }
    }
}
