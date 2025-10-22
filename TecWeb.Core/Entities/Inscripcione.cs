using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TecWeb.Core.Entities;

public partial class Inscripcione
{
    public int InscripcionId { get; set; }

    public int UsuarioId { get; set; }

    public int EventoId { get; set; }

    public DateTime? FechaInscripcion { get; set; }

    public bool? Asistencia { get; set; }

    [JsonIgnore]
    public virtual Evento? Evento { get; set; }

    [JsonIgnore]
    public virtual Usuario? Usuario { get; set; }
}
