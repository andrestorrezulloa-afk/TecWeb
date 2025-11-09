using System;

namespace TecWeb.Infrastructure.DTOs
{
    /// <summary>
    /// DTO que representa una inscripción de un usuario a un evento.
    /// </summary>
    public class InscripcionDto
    {
        /// <summary>
        /// Identificador único de la inscripción.
        /// </summary>
        public int InscripcionId { get; set; }

        /// <summary>
        /// Identificador del usuario que realiza la inscripción.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Identificador del evento al que se inscribe el usuario.
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// Fecha en que se realizó la inscripción.
        /// </summary>
        public DateTime? FechaInscripcion { get; set; }

        /// <summary>
        /// Indica si el usuario asistió al evento.
        /// </summary>
        public bool? Asistencia { get; set; }
    }
}
