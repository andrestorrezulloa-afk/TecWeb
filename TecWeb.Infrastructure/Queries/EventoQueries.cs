using System;
using System.Collections.Generic;

namespace TecWeb.Infrastructure.Queries
{
    public static class EventoQueries
    {
        // 1. Obtener los próximos N eventos ordenados por fecha
        public static string EventosProximosSqlServer = @"
            SELECT EventoId, Titulo, Descripcion, Lugar, Fecha, HoraInicio, HoraFin, AforoMaximo, UsuarioId
            FROM Evento
            WHERE Fecha >= GETDATE()
            ORDER BY Fecha ASC
            OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;";

        // 2. Obtener eventos creados por un usuario específico
        public static string EventosPorUsuarioSqlServer = @"
            SELECT EventoId, Titulo, Descripcion, Lugar, Fecha, HoraInicio, HoraFin, AforoMaximo, UsuarioId
            FROM Evento
            WHERE UsuarioId = @UsuarioId
            ORDER BY Fecha DESC;";

        // 3. Obtener eventos con aforo mayor a un valor específico
        public static string EventosPorAforoSqlServer = @"
            SELECT EventoId, Titulo, Descripcion, Lugar, Fecha, HoraInicio, HoraFin, AforoMaximo, UsuarioId
            FROM Evento
            WHERE AforoMaximo >= @AforoMinimo
            ORDER BY AforoMaximo DESC;";
    }
}
