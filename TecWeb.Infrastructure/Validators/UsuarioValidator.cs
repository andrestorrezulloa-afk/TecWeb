
using System;
using FluentValidation;
using TecWeb.Core.DTOs;

namespace TecWeb.Infrastructure.Validators
{
    public class UsuarioValidator : AbstractValidator<UsuarioDto>
    {
        public UsuarioValidator()
        {
            RuleFor(u => u.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

            RuleFor(u => u.Apellido)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres.");

            RuleFor(u => u.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("Debe ingresar un correo válido.")
                .MaximumLength(150).WithMessage("El correo no puede exceder 150 caracteres.");

            RuleFor(u => u.Telefono)
                .MaximumLength(20).WithMessage("El teléfono no puede exceder 20 caracteres.")
                .When(u => !string.IsNullOrWhiteSpace(u.Telefono));

            RuleFor(u => u.Rol)
                .NotEmpty().WithMessage("El rol es obligatorio.")
                .MaximumLength(50).WithMessage("El rol no puede exceder 50 caracteres.");

            RuleFor(u => u.FechaRegistro)
                .Must(date => !date.HasValue || date.Value <= DateTime.Now)
                .WithMessage("La fecha de registro no puede ser futura.");
        }
    }
}
