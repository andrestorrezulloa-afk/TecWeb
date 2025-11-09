using System;

namespace TecWeb.Core.QueryFilters
{
    public class InscripcionQueryFilter : PaginationQueryFilter
    {
        public int? UsuarioId { get; set; }
        public int? EventoId { get; set; }
        public DateTime? FechaInscripcion { get; set; }
        public bool? Asistencia { get; set; }
    }
}
