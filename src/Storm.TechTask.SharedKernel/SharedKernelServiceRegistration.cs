using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Storm.TechTask.SharedKernel.Authorization;
using Storm.TechTask.SharedKernel.Validation;

namespace Storm.TechTask.SharedKernel
{
    public static class SharedKernelServiceRegistration
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services)
        {
            // MediatR
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(InputValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

            return services;
        }
    }
}
