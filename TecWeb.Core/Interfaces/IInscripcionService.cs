using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.DTOs;
using TecWeb.Core.Services;

namespace TecWeb.Core.Interfaces
{
    public interface IInscripcionService
    {
        Task<ServiceResult<List<InscripcionDto>>> ListarInscripcionesAsync();
        Task<ServiceResult<List<InscripcionDto>>> ListarInscripcionesPorEventoAsync(int eventoId);
        Task<ServiceResult<InscripcionDto>> ObtenerInscripcionPorIdAsync(int id);
        Task<ServiceResult<InscripcionDto>> CrearInscripcionAsync(InscripcionDto dto);
        Task<ServiceResult<InscripcionDto>> ActualizarInscripcionAsync(int id, InscripcionDto dto);
        Task<ServiceResult<bool>> EliminarInscripcionAsync(int id);
    }
}
