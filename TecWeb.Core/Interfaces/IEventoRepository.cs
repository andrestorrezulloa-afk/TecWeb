using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IEventoRepository
    {
        // Métodos de lectura (asíncronos porque usan EF con ToListAsync, FirstOrDefaultAsync, etc.)
        Task<Evento?> ObtenerPorIdAsync(int id);
        Task<List<Evento>> ListarAsync();

        // Crear es asíncrono porque usa AddAsync
        Task<Evento> CrearAsync(Evento evento);

        // Actualizar y eliminar son sincrónicos (solo marcan los cambios)
        void Actualizar(Evento evento);
        void Eliminar(Evento evento);

        // Métodos de validación y comprobación (asíncronos)
        Task<bool> UsuarioExisteAsync(int usuarioId);
        Task<bool> ExisteConflictoAsync(int usuarioId, DateTime fecha, string lugar, int? excludingEventoId = null);
    }
}
