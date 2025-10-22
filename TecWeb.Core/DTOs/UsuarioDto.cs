using System;

namespace TecWeb.Core.DTOs
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = null!;
        public DateTime? FechaRegistro { get; set; }
    }
}
