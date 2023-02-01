using System;
using FluentAssertions;
using M33tingClub.Domain.Utilities;
using M33tingClub.Domain.Utilities.Exceptions;

namespace M33tingClub.UnitTests.Utilities;

internal static class ActionExtensions
{
    internal static void ShouldBrokeRule<TRule>(this Action action)
        where TRule : class, IRule
    {
        action.Should().Throw<RuleValidationException>()
            .Which.BrokenRule.Should().BeOfType<TRule>();
    }
}

internal static class FuncExtensions
{
    internal static void ShouldBrokeRule<TRule>(this Func<object> func)
        where TRule : class, IRule
    {
        func.Should().Throw<RuleValidationException>()
              .Which.BrokenRule.Should().BeOfType<TRule>();
    }
}