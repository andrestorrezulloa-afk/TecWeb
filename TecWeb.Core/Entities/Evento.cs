using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TecWeb.Core.Entities
{
    public partial class Evento
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

        public virtual ICollection<Inscripcione> Inscripciones { get; set; } = new List<Inscripcione>();

        [JsonIgnore]
        public virtual Usuario? Usuario { get; set; }
    }
}
