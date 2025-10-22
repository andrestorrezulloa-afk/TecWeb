using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IInscripcionRepository
    {
        Task<List<Inscripcione>> ListarAsync();           // <- NUEVO (lista todas)
        Task<List<Inscripcione>> ListarPorEventoAsync(int eventoId);
        Task<Inscripcione?> ObtenerPorIdAsync(int id);
        Task<Inscripcione> CrearAsync(Inscripcione entidad);
        Task ActualizarAsync(Inscripcione entidad);
        Task EliminarAsync(Inscripcione entidad);
        Task<bool> UsuarioInscriptoEnEventoAsync(int usuarioId, int eventoId);
    }
}
