using System;
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
    /// <summary>
    /// Servicio que maneja la lógica de negocio para eventos culturales.
    /// </summary>
    public class EventoService : IEventoService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor del servicio de eventos.
        /// </summary>
        /// <param name="unitOfWork">Unidad de trabajo para acceder a los repositorios.</param>
        public EventoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Crea un nuevo evento.
        /// </summary>
        /// <param name="evento">Entidad del evento a crear.</param>
        /// <returns>Resultado del servicio con el evento creado o mensaje de error.</returns>
        public async Task<ServiceResult<Evento>> CrearEventoAsync(Evento evento)
        {
            if (evento == null)
                return ServiceResult<Evento>.Failure("Evento nulo");

            var usuarioExiste = await _unitOfWork.UsuarioRepository.ExistePorIdAsync(evento.UsuarioId);
            if (!usuarioExiste)
                return ServiceResult<Evento>.Failure("Usuario no existe");

            var existeConflicto = await _unitOfWork.EventoRepository.ExisteConflictoAsync(
                evento.UsuarioId, evento.Fecha, evento.Lugar);
            if (existeConflicto)
                return ServiceResult<Evento>.Failure("Conflicto: ya existe un evento del usuario en esa fecha/lugar");

            await _unitOfWork.EventoRepository.CrearAsync(evento);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<Evento>.Success(evento, "Evento creado");
        }

        /// <summary>
        /// Elimina un evento por su Id.
        /// </summary>
        /// <param name="id">Id del evento a eliminar.</param>
        /// <returns>Resultado del servicio indicando éxito o fallo.</returns>
        public async Task<ServiceResult<bool>> EliminarEventoAsync(int id)
        {
            var evento = await _unitOfWork.EventoRepository.ObtenerPorIdAsync(id);
            if (evento == null)
                return ServiceResult<bool>.Failure("Evento no encontrado");

            _unitOfWork.EventoRepository.Eliminar(evento);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<bool>.Success(true, "Evento eliminado");
        }

        /// <summary>
        /// Obtiene un evento por su Id.
        /// </summary>
        /// <param name="id">Id del evento a buscar.</param>
        /// <returns>Resultado del servicio con el evento encontrado o lanza excepción si no existe.</returns>
        public async Task<ServiceResult<Evento>> ObtenerEventoPorIdAsync(int id)
        {
            var evento = await _unitOfWork.EventoRepository.ObtenerPorIdAsync(id);
            if (evento == null)
                throw new BusinessException("El usuario no existe");

            return ServiceResult<Evento>.Success(evento);
        }

        /// <summary>
        /// Lista todos los eventos.
        /// </summary>
        /// <returns>Resultado del servicio con la lista completa de eventos.</returns>
        public async Task<ServiceResult<List<Evento>>> ListarEventosAsync()
        {
            var list = await _unitOfWork.EventoRepository.ListarAsync();
            return ServiceResult<List<Evento>>.Success(list);
        }

        /// <summary>
        /// Actualiza un evento existente.
        /// </summary>
        /// <param name="id">Id del evento a actualizar.</param>
        /// <param name="evento">Entidad con los datos actualizados del evento.</param>
        /// <returns>Resultado del servicio con el evento actualizado o mensaje de error.</returns>
        public async Task<ServiceResult<Evento>> ActualizarEventoAsync(int id, Evento evento)
        {
            var e = await _unitOfWork.EventoRepository.ObtenerPorIdAsync(id);
            if (e == null)
                return ServiceResult<Evento>.Failure("Evento no encontrado");

            e.Titulo = evento.Titulo;
            e.Descripcion = evento.Descripcion;
            e.Lugar = evento.Lugar;
            e.Fecha = evento.Fecha;
            e.HoraInicio = evento.HoraInicio;
            e.HoraFin = evento.HoraFin;
            e.AforoMaximo = evento.AforoMaximo;
            e.UsuarioId = evento.UsuarioId;

            _unitOfWork.EventoRepository.Actualizar(e);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<Evento>.Success(e, "Evento actualizado");
        }

        /// <summary>
        /// Lista eventos filtrados y paginados según los filtros proporcionados.
        /// </summary>
        /// <param name="filters">Filtros para búsqueda y paginación de eventos.</param>
        /// <returns>Resultado del servicio con lista paginada de eventos.</returns>
        public async Task<ServiceResult<PagedList<Evento>>> ListarEventosFiltradosAsync(EventoQueryFilter filters)
        {
            var eventos = await _unitOfWork.EventoRepository.ListarAsync();
            var query = eventos.AsQueryable();

            // Aplicar filtros
            if (filters.UsuarioId != null)
                query = query.Where(e => e.UsuarioId == filters.UsuarioId);

            if (filters.Fecha != null)
                query = query.Where(e => e.Fecha.Date == filters.Fecha.Value.Date);

            if (!string.IsNullOrEmpty(filters.Lugar))
                query = query.Where(e => e.Lugar.ToLower().Contains(filters.Lugar.ToLower()));

            // Aplicar paginación usando PagedList
            var pagedEventos = PagedList<Evento>.Create(query, filters.PageNumber, filters.PageSize);

            return ServiceResult<PagedList<Evento>>.Success(pagedEventos);
        }
    }
}
