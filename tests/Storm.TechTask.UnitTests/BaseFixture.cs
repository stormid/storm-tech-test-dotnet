using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.TechTask.UnitTests.Utilities.Builders.ProjectAggregate;
using Storm.TechTask.UnitTests.Utilities.Comparison;

using Xunit.Abstractions;

namespace Storm.TechTask.UnitTests
{
    public abstract class BaseFixture
    {
        protected ITestOutputHelper Output { get; private set; }

        public BaseFixture(ITestOutputHelper output)
        {
            FluentAssertionsBootstrapper.Bootstrap();
            this.Output = output;
        }


        #region Entity factory

        protected ProjectBuilder NewProject()
        {
            return new ProjectBuilder().WithToDoItems();
        }

        #endregion
    }
}
