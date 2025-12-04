using Microsoft.EntityFrameworkCore;
using TecWeb.Core.Entities;
using TecWeb.Core.Interfaces;  
using TecWeb.Infrastructure.Data;

namespace TecWeb.Infrastructure.Repositories
{
    public class UserSecurityRepository : BaseRepository<UserSecurity>, IUserSecurityRepository
    {
        public UserSecurityRepository(GestionCulturalContext context) : base(context) { }

        public async Task<UserSecurity?> GetLoginByCredentials(UserLogin login)
        {
            // Solo buscar por Login, NO por password
            return await _entities.FirstOrDefaultAsync(x => x.Login == login.User);
        }
    }

    
}