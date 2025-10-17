// Validators/EventoValidator.cs
using FluentValidation;
using TecWeb.DTOs;
using System;

namespace TecWeb.Validators
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
                .Must(date => date >= DateTime.Today)
                    .WithMessage("La fecha del evento no puede ser anterior a hoy.");

            RuleFor(x => x.HoraInicio)
                .NotEmpty().WithMessage("Hora de inicio requerida.");

            RuleFor(x => x.HoraFin)
                .NotEmpty().WithMessage("Hora de fin requerida.")
                .Must((dto, horaFin) => dto.HoraInicio < horaFin)
                .WithMessage("HoraFin debe ser posterior a HoraInicio.");

            RuleFor(x => x.AforoMaximo)
                .GreaterThan(0).WithMessage("El aforo máximo debe ser mayor que 0.");

            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("UsuarioId inválido.");
        }
    }
}