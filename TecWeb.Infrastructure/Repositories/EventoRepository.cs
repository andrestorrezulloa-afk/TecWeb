using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Infrastructure.Data;

namespace TecWeb.Infrastructure.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly GestionCulturalContext _context;

        public EventoRepository(GestionCulturalContext context)
        {
            _context = context;
        }

        public async Task<Evento?> ObtenerPorIdAsync(int id)
        {
            return await _context.Eventos
                .Include(e => e.Inscripciones)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(e => e.EventoId == id);
        }

        public async Task<List<Evento>> ListarAsync()
        {
            return await _context.Eventos
                .Include(e => e.Usuario)
                .Include(e => e.Inscripciones)
                .ToListAsync();
        }

        public async Task<Evento> CrearAsync(Evento evento)
        {
            await _context.Eventos.AddAsync(evento);
            // No guardar cambios aquí: UnitOfWork lo hará
            return evento;
        }

        public void Actualizar(Evento evento)
        {
            _context.Eventos.Update(evento);
        }

        public void Eliminar(Evento evento)
        {
            _context.Eventos.Remove(evento);
        }

        public async Task<bool> UsuarioExisteAsync(int usuarioId)
        {
            return await _context.Usuarios.AnyAsync(u => u.UsuarioId == usuarioId);
        }

        public async Task<bool> ExisteConflictoAsync(int usuarioId, DateTime fecha, string lugar, int? excludingEventoId = null)
        {
            var query = _context.Eventos.AsQueryable();
            if (excludingEventoId.HasValue)
                query = query.Where(e => e.EventoId != excludingEventoId.Value);

            return await query.AnyAsync(e =>
                e.UsuarioId == usuarioId &&
                e.Fecha == fecha &&
                e.Lugar == lugar);
        }
    }
}
