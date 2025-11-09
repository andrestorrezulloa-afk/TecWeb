using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace TecWeb.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        
        IEventoRepository EventoRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        IInscripcionRepository InscripcionRepository { get; }


        Task SaveChangesAsync();
    }
}
