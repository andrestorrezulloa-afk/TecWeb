using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWeb.Core.Entities;

namespace TecWeb.Core.Interfaces
{
    public interface IUserSecurityService
    {
        Task<UserSecurity?> GetLoginByCredentials(UserLogin userLogin);
        Task RegisterUser(UserSecurity userSecurity);
    }
}