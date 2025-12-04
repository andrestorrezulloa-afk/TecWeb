using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWeb.Core.Enum;

namespace TecWeb.Core.Entities
{
    public class UserSecurity
    {
        public int Id { get; set; }
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public RoleType UserRole { get; set; }
    }
}
