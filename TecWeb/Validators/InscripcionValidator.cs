using FluentValidation;
using TecWeb.DTOs;

namespace TecWeb.Validators
{
    public class InscripcionValidator : AbstractValidator<InscripcioneDto>
    {
        public InscripcionValidator()
        {
            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("UsuarioId inválido.");

            RuleFor(x => x.EventoId)
                .GreaterThan(0).WithMessage("EventoId inválido.");

            RuleFor(x => x.FechaInscripcion)
                .Must(date => date == null || date <= DateTime.Now)
                .WithMessage("La fecha de inscripción no puede ser futura.");
        }
    }
}