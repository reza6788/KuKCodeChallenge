using FluentValidation;
using Kuk.Common.Messages;
using Kuk.Services.Services.Note.ViewModel;

namespace Kuk.Services.Services.Note.Validation
{
    public class UpdateNoteValidator : AbstractValidator<NoteUpdateRequestVm>
    {
        public UpdateNoteValidator()
        {
            RuleFor(p => p.Id).NotEmpty().WithMessage(ValidationMessagesResource.IdNotEmpty)
                .GreaterThan(0).WithMessage(ValidationMessagesResource.IdGtZero);
            RuleFor(p => p.Title).NotEmpty().WithMessage(ValidationMessagesResource.TitleNotEmpty)
                .MaximumLength(30).WithMessage(ValidationMessagesResource.InvalidMaxLength);
            RuleFor(p => p.TextBody).NotEmpty().WithMessage(ValidationMessagesResource.TextBodyNotEmpty);

        }
    }
}
