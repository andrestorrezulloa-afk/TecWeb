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

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
            => await _context.Usuarios.FindAsync(id);

        public async Task<Usuario> CrearAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task ActualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Usuario usuario)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistePorIdAsync(int id)
            => await _context.Usuarios.AnyAsync(u => u.UsuarioId == id);

        public async Task<bool> CorreoExisteAsync(string correo)
            => await _context.Usuarios.AnyAsync(u => u.Correo == correo);
    }
}
