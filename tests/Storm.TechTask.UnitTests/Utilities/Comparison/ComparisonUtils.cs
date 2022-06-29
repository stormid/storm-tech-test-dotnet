using System;
using System.Collections.Generic;
using System.Linq;

using Ardalis.GuardClauses;

using FluentAssertions;

namespace Storm.TechTask.UnitTests.Utilities.Comparison
{
    public static class ComparisonUtils
    {
        private static readonly int DateTimePrecision = 5000;

        public static TActual ShouldHaveSameStateAs<TActual, TExpected>(this TActual actual, TExpected expected)
        {
            try
            {

                actual.Should().BeEquivalentTo(expected, options => options
                        // DateTimes will match if within 5 secs.
                        .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(DateTimePrecision)))
                        .WhenTypeIs<DateTime>()
                        // DateTimeOffsets will match if within 5 secs.
                        .Using<DateTimeOffset>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(DateTimePrecision)))
                        .WhenTypeIs<DateTimeOffset>()
                        );
            }
            catch (InvalidOperationException ex)
            {
                // Equivalency check fails if compared objects have no props to compare, but we don't want this behaviour.
                if (!ex.Message.Contains("No members were found for comparison"))
                {
                    throw;
                }
            }

            return actual;
        }

        public static IList<TActual> ShouldHaveSameItemStateAs<TActual, TExpected>(this IList<TActual> actual, IList<TExpected> expected)
        {
            Guard.Against.MismatchingLengths(actual, expected);
            actual.Should().BeEquivalentTo(expected);

            return actual;
        }

        private static void MismatchingLengths<TSource, TExpected>(this IGuardClause _, IEnumerable<TSource> actual, IEnumerable<TExpected> expected)
        {
            if (actual.Count() != expected.Count())
            {
                throw new ArgumentException($"Collections have different lengths: expected is {expected.Count()} while actual is {actual.Count()}");
            }
        }
    }

}
