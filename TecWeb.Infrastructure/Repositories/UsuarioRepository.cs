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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly GestionCulturalContext _context;

        public UsuarioRepository(GestionCulturalContext context)
        {
            _context = context;
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _context.Set<Usuario>().AsEnumerable();
        }

        public async Task<Usuario?> GetById(int id)
        {
            return await _context.Set<Usuario>().FindAsync(id);
        }

        public async Task Add(Usuario entidad)
        {
            await _context.Set<Usuario>().AddAsync(entidad);
            // NO guardar cambios aquí: UnitOfWork lo hará
        }

        public void Update(Usuario entidad)
        {
            _context.Set<Usuario>().Update(entidad);
        }

        public void Delete(Usuario entidad)
        {
            _context.Set<Usuario>().Remove(entidad);
        }

        public async Task<bool> ExistePorIdAsync(int id)
        {
            return await _context.Set<Usuario>().AnyAsync(u => u.UsuarioId == id);
        }

        public async Task<bool> CorreoExisteAsync(string correo)
        {
            return await _context.Set<Usuario>().AnyAsync(u => u.Correo == correo);
        }

        // Ejemplo de método adicional específico de negocio
        public async Task<IEnumerable<Usuario>> BuscarPorNombreAsync(string nombre)
        {
            return await _context.Set<Usuario>()
                .Where(u => u.Nombre.Contains(nombre))
                .ToListAsync();
        }
    }
}
