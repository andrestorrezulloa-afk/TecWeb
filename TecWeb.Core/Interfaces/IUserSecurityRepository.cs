using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IUserSecurityRepository : IBaseRepository<UserSecurity>
    {
        Task<UserSecurity?> GetLoginByCredentials(UserLogin login);
    }
}
