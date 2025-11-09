using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWeb.Core.CustomEntities;
using TecWeb.Core.Entities;
using TecWeb.Core.Exceptions;
using TecWeb.Core.Interfaces;
using TecWeb.Core.QueryFilters;

namespace TecWeb.Core.Services
{
    /// <summary>
    /// Servicio que maneja la lógica de negocio para las inscripciones a eventos.
    /// </summary>
    public class InscripcionService : IInscripcionService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor del servicio de inscripciones.
        /// </summary>
        /// <param name="unitOfWork">Unidad de trabajo para acceder a los repositorios.</param>
        public InscripcionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Lista inscripciones con filtros y paginación opcionales.
        /// </summary>
        /// <param name="filters">Filtros de búsqueda y paginación.</param>
        /// <returns>Lista paginada de inscripciones dentro de un ServiceResult.</returns>
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

            var pageNumber = filters?.PageNumber ?? 1;
            var pageSize = filters?.PageSize ?? 10;

            var pagedInscripciones = PagedList<Inscripcione>.Create(query, pageNumber, pageSize);

            return ServiceResult<PagedList<Inscripcione>>.Success(pagedInscripciones);
        }

        /// <summary>
        /// Lista inscripciones de un evento específico.
        /// </summary>
        /// <param name="eventoId">Id del evento.</param>
        /// <returns>Lista de inscripciones del evento.</returns>
        public async Task<ServiceResult<List<Inscripcione>>> ListarInscripcionesPorEventoAsync(int eventoId)
        {
            var list = await _unitOfWork.InscripcionRepository.ListarPorEventoAsync(eventoId);
            return ServiceResult<List<Inscripcione>>.Success(list);
        }

        /// <summary>
        /// Obtiene una inscripción por su Id.
        /// </summary>
        /// <param name="id">Id de la inscripción.</param>
        /// <returns>Inscripción encontrada o lanza excepción si no existe.</returns>
        public async Task<ServiceResult<Inscripcione>> ObtenerInscripcionPorIdAsync(int id)
        {
            var ins = await _unitOfWork.InscripcionRepository.ObtenerPorIdAsync(id);
            if (ins == null)
                throw new BusinessException("Inscripción no encontrada", 404);

            return ServiceResult<Inscripcione>.Success(ins);
        }

        /// <summary>
        /// Crea una nueva inscripción.
        /// </summary>
        /// <param name="inscripcion">Entidad de la inscripción a crear.</param>
        /// <returns>Inscripción creada o lanza excepción si hay errores.</returns>
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

        /// <summary>
        /// Actualiza una inscripción existente.
        /// </summary>
        /// <param name="id">Id de la inscripción a actualizar.</param>
        /// <param name="inscripcion">Datos actualizados de la inscripción.</param>
        /// <returns>Inscripción actualizada o lanza excepción si no existe.</returns>
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

        /// <summary>
        /// Elimina una inscripción por su Id.
        /// </summary>
        /// <param name="id">Id de la inscripción a eliminar.</param>
        /// <returns>Resultado indicando éxito o lanza excepción si no existe.</returns>
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
