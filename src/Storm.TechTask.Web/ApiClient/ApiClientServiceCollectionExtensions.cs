
using IdentityModel.Client;

using Polly;

using Refit;

namespace Storm.TechTask.Web.ApiClient
{
    public static partial class ApiClientServiceCollectionExtensions
    {
        public static IServiceCollection AddApiClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ApiClientHttpClientHandler>();

            services.AddRefitClient<IProjectApiClient>(new RefitSettings()
                {
                    AuthorizationHeaderValueGetter = async () =>
                    {
                        var client = new HttpClient();
                        var disco = await client.GetDiscoveryDocumentAsync(configuration.GetValue<string>("Authority"));
                        if (disco.IsError)
                        {
                            throw new Exception(disco.Error);
                        }
                        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                        {
                            Address = disco.TokenEndpoint,

                            ClientId = configuration.GetValue<string>("ClientId"),
                            ClientSecret = configuration.GetValue<string>("ClientSecret")
                        });

                        if (tokenResponse.IsError)
                        {
                            throw new Exception(disco.Error);
                        }

                        return tokenResponse.AccessToken;
                    }
                })
                .ConfigureHttpClient(c =>
                {
                    var apiUrl = configuration.GetValue<string>("Api:Projects:Endpoint");
                    c.BaseAddress = new Uri(apiUrl);
                })
                .AddHttpMessageHandler<ApiClientHttpClientHandler>()
                .AddTransientHttpErrorPolicy(policy =>
                {
                    return policy.CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: 2,
                        durationOfBreak: TimeSpan.FromMinutes(1));
                });



            return services;
        }

    }
}
