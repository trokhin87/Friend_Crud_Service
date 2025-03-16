using DTO;
using FluentValidation;

namespace WebLevel.Middleware.Validators;

public class RemoveFriendDtoValidator:AbstractValidator<RemoveFriendDTO>
{
    public RemoveFriendDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AppId is required.");
        RuleFor(x => x.FriendName).NotEmpty().WithMessage("Username is required.");
    }
}