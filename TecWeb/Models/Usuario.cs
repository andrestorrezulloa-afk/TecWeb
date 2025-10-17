using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TecWeb.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Rol { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    [JsonIgnore]
    public virtual ICollection<Evento>? Eventos { get; set; } = new List<Evento>();

    [JsonIgnore]
    public virtual ICollection<Inscripcione>? Inscripciones { get; set; } = new List<Inscripcione>();
}
