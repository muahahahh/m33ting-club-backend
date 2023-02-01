using FluentValidation;

namespace M33tingClub.Application.Meetings.GetMeetings;

public class GetMeetingsForExploreQueryValidator : AbstractValidator<GetMeetingsForExploreQuery>
{
	public GetMeetingsForExploreQueryValidator()
	{
		RuleFor(x => x.Tags).ForEach(y => y.NotEmpty());
		RuleFor(x => x.Limit).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
		RuleFor(x => x.Offset).GreaterThanOrEqualTo(0);
	}
}