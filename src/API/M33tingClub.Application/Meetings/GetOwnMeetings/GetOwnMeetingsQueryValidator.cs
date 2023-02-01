using FluentValidation;

namespace M33tingClub.Application.Meetings.GetOwnMeetings;

public class GetOwnMeetingsQueryValidator : AbstractValidator<GetOwnMeetingsQuery>
{
    public GetOwnMeetingsQueryValidator()
    {
        RuleFor(x => x.Limit).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
        RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
    }
}