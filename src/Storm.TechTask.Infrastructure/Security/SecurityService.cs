using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Storm.TechTask.SharedKernel.Authorization;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Infrastructure.Security
{
    public class SecurityService : ISecurityService
    {
        public IAppUser CurrentUser { get; set; }

        public SecurityService()
        {
            this.CurrentUser = new AnonymousUser();
        }
    }

    internal class AnonymousUser : IAppUser
    {
        public string Username => String.Empty;

        bool IAppUser.HasRole(AppRole role) => false;
    }
}
