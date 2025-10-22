using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.DTOs;
using TecWeb.Core.Services;

namespace TecWeb.Core.Interfaces
{
    public interface IEventoService
    {
        Task<ServiceResult<List<EventoDto>>> ListarEventosAsync();
        Task<ServiceResult<EventoDto>> ObtenerEventoPorIdAsync(int id);
        Task<ServiceResult<EventoDto>> CrearEventoAsync(EventoDto dto);
        Task<ServiceResult<EventoDto>> ActualizarEventoAsync(int id, EventoDto dto);
        Task<ServiceResult<bool>> EliminarEventoAsync(int id);
    }
}
