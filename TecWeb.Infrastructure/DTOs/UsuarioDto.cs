using System;

namespace TecWeb.Infrastructure.DTOs
{
    /// <summary>
    /// DTO que representa un usuario de la aplicación.
    /// </summary>
    public class UsuarioDto
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string Nombre { get; set; } = null!;

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        public string Apellido { get; set; } = null!;

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        public string Correo { get; set; } = null!;

        /// <summary>
        /// Número de teléfono del usuario (opcional).
        /// </summary>
        public string? Telefono { get; set; }

        /// <summary>
        /// Rol del usuario en la aplicación.
        /// </summary>
        public string Rol { get; set; } = null!;

        /// <summary>
        /// Fecha en que el usuario se registró.
        /// </summary>
        public DateTime? FechaRegistro { get; set; }
    }
}
