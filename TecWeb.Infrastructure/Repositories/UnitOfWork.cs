using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TecWeb.Core.Interfaces;
using TecWeb.Infrastructure.Data;

namespace TecWeb.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GestionCulturalContext _context;
        private readonly IDapperContext _dapper;
        private IDbContextTransaction? _efTransaction;

        private IEventoRepository _eventoRepository;
        private IUsuarioRepository _usuarioRepository;
        private IInscripcionRepository _inscripcionRepository;

        public UnitOfWork(GestionCulturalContext context, IDapperContext dapper)
        {
            _context = context;
            _dapper = dapper;
        }

        // Repositorios
        public IEventoRepository EventoRepository =>
            _eventoRepository ??= new EventoRepository(_context);

        public IUsuarioRepository UsuarioRepository =>  
            _usuarioRepository ??= new UsuarioRepository(_context);

        public IInscripcionRepository InscripcionRepository =>
            _inscripcionRepository ??= new InscripcionRepository(_context);

        // Guardar cambios (UnitOfWork controla la transacción)
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Transacciones

        public async Task BeginTransactionAsync()
        {
            if (_efTransaction == null)
            {
                _efTransaction = await _context.Database.BeginTransactionAsync();

                // Registrar la conexión/tx en DapperContext
                var conn = _context.Database.GetDbConnection();
                var tx = _efTransaction.GetDbTransaction();
                _dapper.SetAmbientConnection(conn, tx);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_efTransaction != null)
                {
                    await _efTransaction.CommitAsync();
                    _efTransaction.Dispose();
                    _efTransaction = null;
                }
            }
            finally
            {
                _dapper.ClearAmbientConnection();
            }
        }

        public async Task RollbackAsync()
        {
            if (_efTransaction != null)
            {
                await _efTransaction.RollbackAsync();
                _efTransaction.Dispose();
                _efTransaction = null;
            }
            _dapper.ClearAmbientConnection();
        }

        public IDbConnection? GetDbConnection()
        {
            return _context.Database.GetDbConnection();
        }

        public IDbTransaction? GetDbTransaction()
        {
            return _efTransaction?.GetDbTransaction();
        }

        #endregion

        public void Dispose()
        {
            _efTransaction?.Dispose();
            _context.Dispose();
        }
    }
}
