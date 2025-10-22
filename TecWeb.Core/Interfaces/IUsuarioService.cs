using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.DTOs;
using TecWeb.Core.Services;

namespace TecWeb.Core.Interfaces
{
    public interface IUsuarioService
    {
        Task<ServiceResult<List<UsuarioDto>>> ListarUsuariosAsync();
        Task<ServiceResult<UsuarioDto>> ObtenerUsuarioPorIdAsync(int id);
        Task<ServiceResult<UsuarioDto>> CrearUsuarioAsync(UsuarioDto dto);
        Task<ServiceResult<UsuarioDto>> ActualizarUsuarioAsync(int id, UsuarioDto dto);
        Task<ServiceResult<bool>> EliminarUsuarioAsync(int id);
    }
}
