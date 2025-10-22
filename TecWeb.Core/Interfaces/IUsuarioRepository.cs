using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ListarAsync();                 // <- NUEVO (lista todos)
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario> CrearAsync(Usuario entidad);
        Task ActualizarAsync(Usuario entidad);
        Task EliminarAsync(Usuario entidad);
        Task<bool> ExistePorIdAsync(int id);
        Task<bool> CorreoExisteAsync(string correo);
    }
}
