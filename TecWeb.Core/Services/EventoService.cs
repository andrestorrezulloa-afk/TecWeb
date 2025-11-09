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
    public class EventoService : IEventoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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

        public async Task<ServiceResult<bool>> EliminarEventoAsync(int id)
        {
            var evento = await _unitOfWork.EventoRepository.ObtenerPorIdAsync(id);
            if (evento == null)
                return ServiceResult<bool>.Failure("Evento no encontrado");

            _unitOfWork.EventoRepository.Eliminar(evento);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<bool>.Success(true, "Evento eliminado");
        }

        public async Task<ServiceResult<Evento>> ObtenerEventoPorIdAsync(int id)
        {
            var evento = await _unitOfWork.EventoRepository.ObtenerPorIdAsync(id);
            if (evento == null)
                throw new BusinessException("El usuario no existe");

            return ServiceResult<Evento>.Success(evento);
        }

        public async Task<ServiceResult<List<Evento>>> ListarEventosAsync()
        {
            var list = await _unitOfWork.EventoRepository.ListarAsync();
            return ServiceResult<List<Evento>>.Success(list);
        }

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

        // === FILTRADO Y PAGINACIÓN ===
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

            // Aplicar paginación usando tu PagedList
            var pagedEventos = PagedList<Evento>.Create(query, filters.PageNumber, filters.PageSize);

            return ServiceResult<PagedList<Evento>>.Success(pagedEventos);
        }
    }
}
