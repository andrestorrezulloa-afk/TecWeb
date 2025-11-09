using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;
using TecWeb.Core.QueryFilters;
using TecWeb.Core.CustomEntities; // <- Necesario para PagedList
using TecWeb.Core.Services;

namespace TecWeb.Core.Interfaces
{
    public interface IEventoService
    {
        Task<ServiceResult<List<Evento>>> ListarEventosAsync();
        Task<ServiceResult<Evento>> ObtenerEventoPorIdAsync(int id);
        Task<ServiceResult<Evento>> CrearEventoAsync(Evento evento);
        Task<ServiceResult<Evento>> ActualizarEventoAsync(int id, Evento evento);
        Task<ServiceResult<bool>> EliminarEventoAsync(int id);

        // Cambiado a PagedList para paginación
        Task<ServiceResult<PagedList<Evento>>> ListarEventosFiltradosAsync(EventoQueryFilter filters);
    }
}
