using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario> CrearAsync(Usuario usuario);
        Task ActualizarAsync(Usuario usuario);
        Task EliminarAsync(Usuario usuario);
        Task<bool> ExistePorIdAsync(int id);
        Task<bool> CorreoExisteAsync(string correo);
    }
}
