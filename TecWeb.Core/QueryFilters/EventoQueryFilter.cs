using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecWeb.Core.QueryFilters
{
    public class EventoQueryFilter : PaginationQueryFilter
    {
        public int? UsuarioId { get; set; }
        public DateTime? Fecha { get; set; }
        public string Lugar { get; set; }
    }

}
