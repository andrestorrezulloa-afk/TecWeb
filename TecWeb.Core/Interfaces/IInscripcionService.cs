using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.CustomEntities;
using TecWeb.Core.Entities;
using TecWeb.Core.QueryFilters;
using TecWeb.Core.Services;

namespace TecWeb.Core.Interfaces
{
    public interface IInscripcionService
    {
        // Cambiado a PagedList dentro de ServiceResult para paginación
        Task<ServiceResult<PagedList<Inscripcione>>> ListarInscripcionesAsync(InscripcionQueryFilter filters = null);

        Task<ServiceResult<List<Inscripcione>>> ListarInscripcionesPorEventoAsync(int eventoId);
        Task<ServiceResult<Inscripcione>> ObtenerInscripcionPorIdAsync(int id);
        Task<ServiceResult<Inscripcione>> CrearInscripcionAsync(Inscripcione dto);
        Task<ServiceResult<Inscripcione>> ActualizarInscripcionAsync(int id, Inscripcione dto);
        Task<ServiceResult<bool>> EliminarInscripcionAsync(int id);
    }
}
