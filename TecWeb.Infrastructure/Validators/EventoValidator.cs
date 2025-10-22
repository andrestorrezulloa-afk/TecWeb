// Validators/EventoValidator.cs
using System;
using FluentValidation;
using TecWeb.Core.DTOs;

namespace TecWeb.Infrastructure.Validators
{
    public class EventoValidator : AbstractValidator<EventoDto>
    {
        public EventoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("El título es requerido.")
                .MaximumLength(200).WithMessage("El título no puede superar 200 caracteres.");

            RuleFor(x => x.Lugar)
                .NotEmpty().WithMessage("El lugar es requerido.")
                .MaximumLength(200).WithMessage("El lugar no puede superar 200 caracteres.");

            RuleFor(x => x.Descripcion)
                .MaximumLength(1000).WithMessage("La descripción no puede superar 1000 caracteres.");

            RuleFor(x => x.Fecha)
                .Must(date => date != default(DateTime))
                    .WithMessage("Fecha inválida.")
                .Must(date => date.Date >= DateTime.Today)
                    .WithMessage("La fecha del evento no puede ser anterior a hoy.");

            RuleFor(x => x.HoraInicio)
                .Must(h => h != default(DateTime))
                    .WithMessage("Hora de inicio requerida.");

            RuleFor(x => x.HoraFin)
                .Must(h => h != default(DateTime))
                    .WithMessage("Hora de fin requerida.")
                .Must((dto, horaFin) =>
                {
                    
                    if (dto.HoraInicio == default || horaFin == default) return false;
                    return dto.HoraInicio.TimeOfDay < horaFin.TimeOfDay;
                })
                .WithMessage("HoraFin debe ser posterior a HoraInicio.");

            RuleFor(x => x.AforoMaximo)
                .GreaterThan(0).WithMessage("El aforo máximo debe ser mayor que 0.");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("UsuarioId inválido.");
        }
    }
}
