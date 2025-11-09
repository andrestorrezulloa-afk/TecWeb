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

        public async Task<List<Inscripcione>> ListarAsync()
        {
            return await _context.Inscripciones
                .Include(i => i.Usuario)
                .Include(i => i.Evento)
                .ToListAsync();
        }

        public async Task<List<Inscripcione>> ListarPorEventoAsync(int eventoId)
        {
            return await _context.Inscripciones
                .Where(i => i.EventoId == eventoId)
                .Include(i => i.Usuario)
                .Include(i => i.Evento)
                .ToListAsync();
        }

        public async Task<Inscripcione?> ObtenerPorIdAsync(int id)
        {
            return await _context.Inscripciones
                .Include(i => i.Usuario)
                .Include(i => i.Evento)
                .FirstOrDefaultAsync(i => i.InscripcionId == id);
        }

        public async Task CrearAsync(Inscripcione entidad)
        {
            await _context.Inscripciones.AddAsync(entidad);
            // No guardar cambios aquí: lo hace UnitOfWork
        }

        public void Actualizar(Inscripcione entidad)
        {
            _context.Inscripciones.Update(entidad);
        }

        public void Eliminar(Inscripcione entidad)
        {
            _context.Inscripciones.Remove(entidad);
        }

        public async Task<bool> UsuarioInscriptoEnEventoAsync(int usuarioId, int eventoId)
        {
            return await _context.Inscripciones
                .AnyAsync(i => i.UsuarioId == usuarioId && i.EventoId == eventoId);
        }
    }
}
