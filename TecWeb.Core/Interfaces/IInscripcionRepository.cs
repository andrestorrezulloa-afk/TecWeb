using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IInscripcionRepository
    {
        Task<Inscripcione?> ObtenerPorIdAsync(int id);
        Task<List<Inscripcione>> ListarPorEventoAsync(int eventoId);
        Task<Inscripcione> CrearAsync(Inscripcione inscripcion);
        Task ActualizarAsync(Inscripcione inscripcion); 
        Task EliminarAsync(Inscripcione inscripcion);

        Task<bool> UsuarioInscriptoEnEventoAsync(int usuarioId, int eventoId);
    }
}
