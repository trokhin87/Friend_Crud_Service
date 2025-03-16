using DTO;
using FluentValidation;

namespace WebLevel.Middleware.Validators;

public class FriendDtoValidator: AbstractValidator<FriendDTO>
{
    public FriendDtoValidator()
    {
        RuleFor(x => x.AppId).NotEmpty().WithMessage("AppId is required.");
        RuleFor(x => x.FriendUsername).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.FriendName).NotEmpty().WithMessage("Friend name is required.");
        RuleFor(x => x.DateBirth).LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must be in the past.");
    }
}