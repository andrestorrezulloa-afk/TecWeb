using System;

namespace TecWeb.Infrastructure.DTOs
{
    /// <summary>
    /// DTO que representa un evento cultural en la API.
    /// </summary>
    public class EventoDto
    {
        /// <summary>
        /// Identificador único del evento.
        /// </summary>
        public int EventoId { get; set; }

        /// <summary>
        /// Título del evento.
        /// </summary>
        public string Titulo { get; set; } = null!;

        /// <summary>
        /// Descripción del evento.
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Lugar donde se realiza el evento.
        /// </summary>
        public string Lugar { get; set; } = null!;

        /// <summary>
        /// Fecha del evento.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Hora de inicio del evento.
        /// </summary>
        public DateTime HoraInicio { get; set; }

        /// <summary>
        /// Hora de fin del evento.
        /// </summary>
        public DateTime HoraFin { get; set; }

        /// <summary>
        /// Capacidad máxima de asistentes.
        /// </summary>
        public int AforoMaximo { get; set; }

        /// <summary>
        /// Identificador del usuario creador del evento.
        /// </summary>
        public int UsuarioId { get; set; }
    }
}
