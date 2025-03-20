using DTO;
using FluentValidation;

namespace WebLevel.Middleware.Validators;

public class RemoveFriendDtoValidator:AbstractValidator<DeleteFriendDto>
{
    public RemoveFriendDtoValidator()
    {
        RuleFor(x => x.AppId).NotEmpty().WithMessage("AppId is required.");
        RuleFor(x => x.FriendName).NotEmpty().WithMessage("Username is required.");
    }
}