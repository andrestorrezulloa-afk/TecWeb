// Services/IInscripcionService.cs
using TecWeb.DTOs;

namespace TecWeb.Services
{
    public interface IInscripcionService
    {
        Task<ServiceResult<InscripcioneDto>> CrearInscripcionAsync(InscripcioneDto inscripcionDto);
        Task<ServiceResult<InscripcioneDto>> ActualizarInscripcionAsync(int id, InscripcioneDto inscripcionDto);
        Task<ServiceResult<bool>> EliminarInscripcionAsync(int id);
        Task<ServiceResult<List<InscripcioneDto>>> ListarInscripcionesAsync();
        Task<ServiceResult<InscripcioneDto>> ObtenerInscripcionPorIdAsync(int id);
    }
}