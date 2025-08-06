using ContactsApi.Dtos;
using ContactsApi.Services;
using FluentValidation;

namespace ContactsApi.Validators;

public class UpdateContactValidator : AbstractValidator<UpdateContactDto>
{
    public UpdateContactValidator(ContactService contactService)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z\-\s]+$")
            .WithMessage("First name must contain only letters, hyphens, or spaces.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z\-\s]+$")
            .WithMessage("Last name must contain only letters, hyphens, or spaces.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+998\d{9}$")
            .WithMessage("Phone number must be in +998XXXXXXXXX format.");

        RuleFor(x => x.Address)
            .MinimumLength(5)
            .MaximumLength(200)
            .When(x => x.Address != null);
    }
}