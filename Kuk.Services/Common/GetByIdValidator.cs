using FluentValidation;

namespace Kuk.Services.Common
{
    public class GetByIdValidator : AbstractValidator<int>
    {
        public GetByIdValidator()
        {
            RuleFor(p => p).NotEmpty().WithMessage("ID must not be empty")
                .GreaterThan(0).WithMessage("ID must be greater than zero");
        }
    }
}
