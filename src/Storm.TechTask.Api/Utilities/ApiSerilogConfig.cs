using Serilog;

using Storm.TechTask.Infrastructure.Logging;

namespace Storm.TechTask.Api.Utilities
{
    public static class ApiSerilogConfig
    {
        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.SetupCommonConfig(context));

            return builder;
        }

        public static WebApplication UseRequestLogging(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            return app;
        }
    }
}
