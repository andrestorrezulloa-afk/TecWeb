using System;
using System.Collections.Generic;

namespace TecWeb.Infrastructure.Queries
{
    public static class InscripcionQueries
    {
        // 1. Obtener todas las inscripciones más recientes (últimas N)
        public static string InscripcionesRecientesSqlServer = @"
            SELECT InscripcionId, UsuarioId, EventoId, FechaInscripcion, Asistencia
            FROM Inscripcion
            ORDER BY FechaInscripcion DESC
            OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;";

        // 2. Obtener inscripciones de un usuario específico
        public static string InscripcionesPorUsuarioSqlServer = @"
            SELECT InscripcionId, UsuarioId, EventoId, FechaInscripcion, Asistencia
            FROM Inscripcion
            WHERE UsuarioId = @UsuarioId
            ORDER BY FechaInscripcion DESC;";

        // 3. Obtener inscripciones de un evento específico con asistencia confirmada
        public static string InscripcionesAsistentesEventoSqlServer = @"
            SELECT InscripcionId, UsuarioId, EventoId, FechaInscripcion, Asistencia
            FROM Inscripcion
            WHERE EventoId = @EventoId AND Asistencia = 1
            ORDER BY FechaInscripcion ASC;";
    }
}
