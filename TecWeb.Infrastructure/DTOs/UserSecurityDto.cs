using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWeb.Core.Enum;

namespace TecWeb.Infrastructure.DTOs
{
    public class UserSecurityDto
    {
        public string FullName { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public RoleType? UserRole { get; set; }
    }
}