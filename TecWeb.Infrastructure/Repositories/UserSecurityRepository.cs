using Microsoft.EntityFrameworkCore;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;  // Ahora usa la interfaz de Core
using TecWeb.Infrastructure.Data;

namespace TecWeb.Infrastructure.Repositories
{
    public class UserSecurityRepository : BaseRepository<UserSecurity>, IUserSecurityRepository
    {
        public UserSecurityRepository(GestionCulturalContext context) : base(context) { }

        public async Task<UserSecurity?> GetLoginByCredentials(UserLogin login)
        {
            return await _entities
                .FirstOrDefaultAsync(x => x.Login == login.User
                    && x.PasswordHash == login.Password);
        }
    }

    // ¡ELIMINA la interfaz de este archivo! Ya está en Core
}