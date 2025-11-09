using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecWeb.Core.Enum;
namespace TecWeb.Core.Interfaces
{
    public interface IDbConnectionFactory
    {
        DatabaseProvider Provider { get; }
        IDbConnection CreateConnection();
    }

}
