using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Storm.TechTask.SharedKernel.Authorization;

namespace Storm.TechTask.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(typeof(CoreServiceRegistration));
            services.AddAuthorizersFromAssemblyContaining(typeof(CoreServiceRegistration));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
