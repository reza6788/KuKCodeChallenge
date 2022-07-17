using FluentValidation;
using Kuk.Common.Messages;
using Kuk.Services.Services.Note.ViewModel;

namespace Kuk.Services.Services.Note.Validation
{
    public class AddNoteValidator : AbstractValidator<NoteCreateRequestVm>
    {
        public AddNoteValidator()
        {
            RuleFor(p => p.Title).NotEmpty().WithMessage(ValidationMessagesResource.TitleNotEmpty)
                .MaximumLength(50).WithMessage(ValidationMessagesResource.InvalidMaxLength);
            RuleFor(p => p.TextBody).NotEmpty().WithMessage(ValidationMessagesResource.TextBodyNotEmpty);

        }
    }
}
