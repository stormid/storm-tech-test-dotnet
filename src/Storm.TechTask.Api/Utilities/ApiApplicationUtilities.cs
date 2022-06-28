using Storm.TechTask.Api.Database;
using Storm.TechTask.Api.Utilities.IdentityServer;
using Storm.TechTask.Infrastructure.Repository;

namespace Storm.TechTask.Api.Utilities
{
    public static class ApiApplicationUtilities
    {

        public static WebApplication InitializeDatabase(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                try
                {
                    using (var scope = app.Services.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        // TODO: Replace this with something both more convenient & more robust (e.g. cake task for db init, cake task for migrations etc?)
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                        SeedData.Initialize(app.Environment, scope.ServiceProvider);
                    }
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            return app;
        }

        public static WebApplication SetUpRequestPipeline(this WebApplication app, IConfigurationRoot configuration)
        {
            app.UseRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                //app.UseShowAllServicesMiddleware();
                //app.DisplayRoutes("/debug/routes");
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Storm.NET V1");
                c.UseOAuthOptions(configuration);
            });
            }

            app.MapDefaultControllerRoute();
            app.UseIdProvider(app.Environment);
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
