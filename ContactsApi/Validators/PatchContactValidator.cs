using ContactsApi.Services;
using FluentValidation;
using ContactsApi.Dtos;

namespace ContactsApi.Validators;

public class PatchContactValidator : AbstractValidator<PatchContactDto>

{
    public PatchContactValidator(ContactService contactService)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z\-\s]+$")
            .WithMessage("First name must contain only letters, hyphens, or spaces.")
            .When(x => x.FirstName != null);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z\-\s]+$")
            .WithMessage("Last name must contain only letters, hyphens, or spaces.")
            .When(x => x.LastName != null);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .When(x => x.Email != null);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+998\d{9}$")
            .WithMessage("Phone number must be in +998XXXXXXXXX format.")
            .When(x => x.PhoneNumber != null);

        RuleFor(x => x.Address)
            .MinimumLength(5)
            .MaximumLength(200)
            .When(x => x.Address != null);
    }
}