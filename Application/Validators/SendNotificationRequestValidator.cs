using FluentValidation;
using NotificationAPI.Application.DTOs;

namespace NotificationAPI.Application.Validators;

public class SendNotificationRequestValidator : AbstractValidator<SendNotificationRequest>
{
    public SendNotificationRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId debe ser mayor que 0");

        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Subject es requerido")
            .MaximumLength(200)
            .WithMessage("Subject no puede exceder 200 caracteres");

        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message es requerido")
            .MaximumLength(5000)
            .WithMessage("Message no puede exceder 5000 caracteres");
    }
}
