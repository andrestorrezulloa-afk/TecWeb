using System;

namespace TecWeb.Core.DTOs
{
    public class EventoDto
    {
        public int EventoId { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string Lugar { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFin { get; set; }
        public int AforoMaximo { get; set; }
        public int UsuarioId { get; set; }
    }
}
