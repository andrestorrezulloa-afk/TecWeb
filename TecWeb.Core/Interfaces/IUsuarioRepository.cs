using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();              // Para obtener todos los usuarios sin async
        Task<Usuario?> GetById(int id);             // Obtener por id async
        Task Add(Usuario entidad);                  // Agregar nuevo usuario
        void Update(Usuario entidad);               // Actualizar
        void Delete(Usuario entidad);               // Eliminar
        Task<bool> ExistePorIdAsync(int id);        // Verificar si existe
        Task<bool> CorreoExisteAsync(string correo); // Verificar correo
    }
}
