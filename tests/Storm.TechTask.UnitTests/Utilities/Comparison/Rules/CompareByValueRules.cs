using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions.Equivalency;

namespace Storm.TechTask.UnitTests.Utilities.Comparison.Rules
{
    public class CompareByValueRules : IMemberSelectionRule
    {
        public bool IncludesMembers => true;

        public IEnumerable<IMember> SelectMembers(INode currentNode, IEnumerable<IMember> selectedMembers, MemberSelectionContext context)
        {
            var compileTimeIncludes = new List<IMember>();

            context.Type.GetFields()
                .Where(f => !f.Name.Contains("k__backingField"))
                .Where(f => !f.Name.Contains("_entityWrapper"))
                .ToList()?
                .ForEach(f => compileTimeIncludes.Add(MemberFactory.Create(f, currentNode)));

            context.Type.GetProperties()?
                .ToList()
                .ForEach(p => compileTimeIncludes.Add(MemberFactory.Create(p, currentNode)));

            return compileTimeIncludes;
        }
    }
}
