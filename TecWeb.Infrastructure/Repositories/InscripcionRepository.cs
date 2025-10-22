using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Infrastructure.Data;

namespace TecWeb.Infrastructure.Repositories
{
    public class InscripcionRepository : IInscripcionRepository
    {
        private readonly GestionCulturalContext _context;
        public InscripcionRepository(GestionCulturalContext context)
        {
            _context = context;
        }

        public async Task<Inscripcione?> ObtenerPorIdAsync(int id)
            => await _context.Inscripciones.FindAsync(id);

        public async Task<List<Inscripcione>> ListarPorEventoAsync(int eventoId)
            => await _context.Inscripciones
                .Where(i => i.EventoId == eventoId)
                .Include(i => i.Usuario)
                .ToListAsync();

        public async Task<Inscripcione> CrearAsync(Inscripcione inscripcion)
        {
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();
            return inscripcion;
        }

        public async Task ActualizarAsync(Inscripcione inscripcion)
        {
            _context.Inscripciones.Update(inscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Inscripcione inscripcion)
        {
            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UsuarioInscriptoEnEventoAsync(int usuarioId, int eventoId)
            => await _context.Inscripciones.AnyAsync(i => i.UsuarioId == usuarioId && i.EventoId == eventoId);
    }
}
