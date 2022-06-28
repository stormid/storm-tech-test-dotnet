using Microsoft.OpenApi.Models;

using Storm.TechTask.Core;
using Storm.TechTask.Infrastructure;
using Storm.TechTask.Infrastructure.Repository;
using Storm.TechTask.SharedKernel;
using Storm.TechTask.SharedKernel.Handlers;
using Microsoft.EntityFrameworkCore;
using Storm.TechTask.Api.Utilities.WebSession;
using Storm.TechTask.SharedKernel.Interfaces;
using Storm.TechTask.Api.Utilities.IdentityServer;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Storm.TechTask.Api.Filters;

namespace Storm.TechTask.Api.Utilities
{
    public static class ApiApplicationBuilderUtilities
    {
        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("SqliteConnection");  //Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

            return builder;
        }

        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdProvider(builder.Environment, builder.Configuration);
            builder.Services.AddSharedKernel();
            builder.Services.AddCore();
            builder.Services.AddInfrastructure(builder.Configuration);

            return builder;
        }

        public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
        {
            // Telemetry
            builder.Services.AddApplicationInsightsTelemetry();

            // Session (see https://andrewlock.net/how-to-register-a-service-with-multiple-interfaces-for-in-asp-net-core-di/)
            builder.Services.AddSingleton<NoOpWebSession>();
            builder.Services.AddSingleton<IUserSession>(sp => sp.GetRequiredService<NoOpWebSession>());
            builder.Services.AddSingleton<IReadUserSession>(sp => sp.GetRequiredService<NoOpWebSession>());

            // Controllers
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter()); // Apply ASP.NET [Authorize] to all endpoints.
                options.Filters.Add<AuthenticationFilter>();
                options.Filters.Add<InputValidationFilter>();
                options.Filters.Add<AuthorizationFilter>();
                options.Filters.Add<BusinessRuleExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Storm.NET API", Version = "v1" });
                c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" }); // TODO [GJL] Drop this if/when Swagger natively supports DateOnly.
                c.CustomSchemaIds(type => type.FormatSwaggerSchemaId());
                c.EnableAnnotations();
                c.AddOAuthOptions(builder.Configuration);
            });

            return builder;
        }

        public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            return builder;
        }

        private static string? FormatSwaggerSchemaId(this Type endpointParameterType)
        {
            if (endpointParameterType.IsAssignableTo(typeof(IAction)))
            {
                // This is necessary coz several endpoints have param types which are nested classes with the same short name (Command or Query) - and the short name is the default Swagger schemaId.
                // So by default, a type like Storm.DotNet.Core.ProjectAggregate.Queries.AllProjects+Query becomes simply "Query", which causes clashes.
                // We want a type like this to use AllProjectsQuery as its Swagger SchemaId (which is what would have been the deault prior to our introduction of nested classes).
                return endpointParameterType.FormatActionName();
            }
            else
            {
                return endpointParameterType.Name;
            }
        }
    }
}
