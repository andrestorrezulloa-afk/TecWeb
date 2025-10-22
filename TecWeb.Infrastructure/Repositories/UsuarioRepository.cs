using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using TecWeb.Infrastructure.Data;

namespace TecWeb.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GestionCulturalContext _context;
        public UsuarioRepository(GestionCulturalContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ListarAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
            => await _context.Usuarios.FindAsync(id);

        public async Task<Usuario> CrearAsync(Usuario entidad)
        {
            _context.Usuarios.Add(entidad);
            await _context.SaveChangesAsync();
            return entidad;
        }

        public async Task ActualizarAsync(Usuario entidad)
        {
            _context.Usuarios.Update(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Usuario entidad)
        {
            _context.Usuarios.Remove(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistePorIdAsync(int id)
            => await _context.Usuarios.AnyAsync(u => u.UsuarioId == id);

        public async Task<bool> CorreoExisteAsync(string correo)
            => await _context.Usuarios.AnyAsync(u => u.Correo == correo);
    }
}
