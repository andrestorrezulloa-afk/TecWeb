using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IInscripcionRepository
    {
        Task<List<Inscripcione>> ListarAsync();
        Task<List<Inscripcione>> ListarPorEventoAsync(int eventoId);
        Task<Inscripcione?> ObtenerPorIdAsync(int id);
        Task CrearAsync(Inscripcione entidad);
        void Actualizar(Inscripcione entidad);
        void Eliminar(Inscripcione entidad);
        Task<bool> UsuarioInscriptoEnEventoAsync(int usuarioId, int eventoId);
    }
}
