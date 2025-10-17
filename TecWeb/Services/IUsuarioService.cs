using TecWeb.DTOs;

namespace TecWeb.Services
{
    public interface IUsuarioService
    {
        Task<ServiceResult<UsuarioDto>> CrearUsuarioAsync(UsuarioDto usuarioDto);
        Task<ServiceResult<UsuarioDto>> ActualizarUsuarioAsync(int id, UsuarioDto usuarioDto);
        Task<ServiceResult<bool>> EliminarUsuarioAsync(int id);
        Task<ServiceResult<List<UsuarioDto>>> ListarUsuariosAsync();
        Task<ServiceResult<UsuarioDto>> ObtenerUsuarioPorIdAsync(int id);
    }
}