using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Storm.TechTask.SharedKernel.Authorization;

using Xunit;

namespace Storm.TechTask.Api.IntegrationTests
{
    public class AllRolesExceptAttribute : MemberDataAttributeBase
    {
        private readonly AppRole[] _roles;

        public AllRolesExceptAttribute(params AppRole[] roles)
            : base("", Array.Empty<object>())
        {
            _roles = roles;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var validRoles = Enum.GetValues<AppRole>().ToList()
                .Except(new[] { AppRole.Anonymous, AppRole.MAX }); // Token issuer isn't configured to support these roles

            foreach (var appRole in validRoles.Where(r => !_roles.Contains(r)))
            {
                yield return new object[] { appRole };
            }
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            return Array.Empty<object>();
        }
    }

}
