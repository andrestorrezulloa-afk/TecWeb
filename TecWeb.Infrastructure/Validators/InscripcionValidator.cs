// Validators/InscripcionValidator.cs
using System;
using FluentValidation;
using TecWeb.Core.DTOs;

namespace TecWeb.Infrastructure.Validators
{
    public class InscripcionValidator : AbstractValidator<InscripcionDto>
    {
        public InscripcionValidator()
        {
            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("UsuarioId inválido.");

            RuleFor(x => x.EventoId)
                .GreaterThan(0).WithMessage("EventoId inválido.");

            RuleFor(x => x.FechaInscripcion)
                .Must(date => !date.HasValue || date.Value <= DateTime.Now)
                .WithMessage("La fecha de inscripción no puede ser futura.");
        }
    }
}
