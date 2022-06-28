using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.TechTask.SharedKernel.Authorization
{

    public interface IAppUser
    {
        string Username { get; }

        bool HasRole(AppRole role);
    }
}
