using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using Storm.TechTask.Infrastructure.Logging;
using Storm.TechTask.Infrastructure.Repository;
using Storm.TechTask.Infrastructure.Security;
using Storm.TechTask.SharedKernel.Interfaces;

namespace Storm.TechTask.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // EF (see https://andrewlock.net/how-to-register-a-service-with-multiple-interfaces-for-in-asp-net-core-di/)
            services.AddScoped<EfRepository>();
            services.AddScoped<IRepository>(sp => sp.GetRequiredService<EfRepository>());
            services.AddScoped<IReadRepository>(sp => sp.GetRequiredService<EfRepository>());

            // Infra services
            services.AddSingleton<ILoggingService, LoggingService>((serviceProvider) =>
                new LoggingService(Log.Logger, SerilogConfig.GetSecurityLogger(configuration)));
            services.AddSingleton<IClock, Clock.Clock>();
            services.AddScoped<ISecurityService, SecurityService>();

            return services;
        }
    }
}
