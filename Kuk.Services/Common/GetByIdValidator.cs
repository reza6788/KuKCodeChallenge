using FluentValidation;
using Kuk.Common.Messages;

namespace Kuk.Services.Common
{
    public class GetByIdValidator : AbstractValidator<int>
    {
        public GetByIdValidator()
        {
            RuleFor(p => p).NotEmpty().WithMessage(ValidationMessagesResource.IdNotEmpty)
                .GreaterThan(0).WithMessage(ValidationMessagesResource.IdGtZero);
        }
    }
}
