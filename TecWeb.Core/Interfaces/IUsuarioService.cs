
using TecWeb.Core.Entities;
using TecWeb.Core.Services;
namespace TecWeb.Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<ServiceResult<List<Usuario>>> ListarUsuariosAsync();
        Task<ServiceResult<Usuario>> ObtenerUsuarioPorIdAsync(int id);
        Task<ServiceResult<Usuario>> CrearUsuarioAsync(Usuario dto);
        Task<ServiceResult<Usuario>> ActualizarUsuarioAsync(int id, Usuario dto);
        Task<ServiceResult<bool>> EliminarUsuarioAsync(int id);
    }
}
