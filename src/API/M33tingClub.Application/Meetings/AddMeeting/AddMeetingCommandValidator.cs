using FluentValidation;

namespace M33tingClub.Application.Meetings.AddMeeting;

public class AddMeetingCommandValidator : AbstractValidator<AddMeetingCommand>
{
    public AddMeetingCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Description).MaximumLength(255);
        RuleFor(x => x.ImageId).NotEmpty();
    }
}