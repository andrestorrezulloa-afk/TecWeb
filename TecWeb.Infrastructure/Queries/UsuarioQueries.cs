using System;
using System.Collections.Generic;

namespace TecWeb.Infrastructure.Queries
{
    public static class UsuarioQueries
    {
        // 1. Obtener los usuarios registrados más recientemente (últimos N)
        public static string UsuariosRecientesSqlServer = @"
            SELECT UsuarioId, Nombre, Apellido, Correo, Telefono, Rol, FechaRegistro
            FROM Usuario
            ORDER BY FechaRegistro DESC
            OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;";

        // 2. Obtener usuarios por rol específico
        public static string UsuariosPorRolSqlServer = @"
            SELECT UsuarioId, Nombre, Apellido, Correo, Telefono, Rol, FechaRegistro
            FROM Usuario
            WHERE Rol = @Rol
            ORDER BY Nombre, Apellido;";

        // 3. Obtener usuarios con más de X inscripciones a eventos
        public static string UsuariosConInscripcionesSqlServer = @"
            SELECT u.UsuarioId, u.Nombre, u.Apellido, u.Correo, u.Telefono, u.Rol, u.FechaRegistro, COUNT(i.InscripcionId) AS TotalInscripciones
            FROM Usuario u
            LEFT JOIN Inscripcion i ON u.UsuarioId = i.UsuarioId
            GROUP BY u.UsuarioId, u.Nombre, u.Apellido, u.Correo, u.Telefono, u.Rol, u.FechaRegistro
            HAVING COUNT(i.InscripcionId) > @MinInscripciones
            ORDER BY TotalInscripciones DESC;";
    }
}
