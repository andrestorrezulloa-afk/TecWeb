using TecWeb.DTOs;

namespace TecWeb.Services
{
    public interface IEventoService
    {
        Task<ServiceResult<EventoDto>> CrearEventoAsync(EventoDto eventoDto);
        Task<ServiceResult<EventoDto>> ActualizarEventoAsync(int id, EventoDto eventoDto);
        Task<ServiceResult<bool>> EliminarEventoAsync(int id);
        Task<ServiceResult<List<EventoDto>>> ListarEventosAsync();
        Task<ServiceResult<EventoDto>> ObtenerEventoPorIdAsync(int id);
    }
}