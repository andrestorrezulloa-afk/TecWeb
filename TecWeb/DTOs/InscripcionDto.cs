using System;

namespace TecWeb.DTOs
{
    public class InscripcioneDto
    {
        public int InscripcionId { get; set; }
        public int UsuarioId { get; set; }
        public int EventoId { get; set; }
        public DateTime? FechaInscripcion { get; set; }
        public bool? Asistencia { get; set; }
    }
}