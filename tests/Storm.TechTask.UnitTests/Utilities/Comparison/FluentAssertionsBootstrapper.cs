using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Storm.TechTask.UnitTests.Utilities.Comparison.Rules;

namespace Storm.TechTask.UnitTests.Utilities.Comparison
{
    public class FluentAssertionsBootstrapper
    {
        public static void Bootstrap() => AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using(new CompareByValueRules());

            return options;
        });
    }
}
