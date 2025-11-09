using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TecWeb.Core.Interfaces;
using TecWeb.Infrastructure.Data;

namespace TecWeb.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly GestionCulturalContext _context;
        protected readonly IDapperContext? _dapper;
        protected readonly DbSet<T> _entities;

        public BaseRepository(GestionCulturalContext context, IDapperContext? dapper = null)
        {
            _context = context;
            _dapper = dapper;
            _entities = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _entities.Remove(entity);
        }

        // 👇 Esto devuelve la conexión solo si existe DapperContext
        public IDbConnection? GetConnection()
        {
            return _dapper?.CreateConnection();
        }
    }
}
