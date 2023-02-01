using FluentValidation;

namespace M33tingClub.Application.Meetings.EditMeeting;

public class EditMeetingCommandValidator : AbstractValidator<EditMeetingCommand>
{
    public EditMeetingCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Description).MaximumLength(255);
        RuleFor(x => x.ImageId).NotEmpty();
    }
}