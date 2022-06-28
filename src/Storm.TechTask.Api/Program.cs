using Serilog;

using Storm.TechTask.Api.Utilities;
using Storm.TechTask.Infrastructure.Logging;

SerilogConfig.AddBootstrapLogging();

try
{
    //
    // Builder config.
    //
    var builder = WebApplication.CreateBuilder(args);
    builder.AddLogging()
        .AddServices()
        .AddDatabase()
        .AddApi()
        .AddConfiguration();

    //
    // App config.
    //
    var app = builder.Build();
    app.SetUpRequestPipeline(builder.Configuration)
        .InitializeDatabase();

    //
    // App run.
    //
    Log.Information("Starting api app");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Api app terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
}




