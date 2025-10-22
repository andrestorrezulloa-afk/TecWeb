using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IEventoRepository
    {
        Task<Evento?> ObtenerPorIdAsync(int id);
        Task<List<Evento>> ListarAsync();
        Task<Evento> CrearAsync(Evento evento);
        Task ActualizarAsync(Evento evento);
        Task EliminarAsync(Evento evento);

        
        Task<bool> UsuarioExisteAsync(int usuarioId);
        Task<bool> ExisteConflictoAsync(int usuarioId, DateTime fecha, string lugar, int? excludingEventoId = null);
    }
}
