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
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task ActualizarAsync(Evento evento)
        {
            _context.Eventos.Update(evento);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Evento evento)
        {
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsuarioExisteAsync(int usuarioId)
        {
            return await _context.Usuarios.AnyAsync(u => u.UsuarioId == usuarioId);
        }

        public async Task<bool> ExisteConflictoAsync(int usuarioId, DateTime fecha, string lugar, int? excludingEventoId = null)
        {
            var q = _context.Eventos.AsQueryable();
            if (excludingEventoId.HasValue)
                q = q.Where(e => e.EventoId != excludingEventoId.Value);

            return await q.AnyAsync(e => e.UsuarioId == usuarioId && e.Fecha == fecha && e.Lugar == lugar);
        }
    }
}
