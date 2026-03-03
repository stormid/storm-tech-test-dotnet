
using Duende.IdentityModel.Client;

using Microsoft.Extensions.Http.Resilience;

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
                    AuthorizationHeaderValueGetter = async (request, cancellationToken) =>
                    {
                        var client = new HttpClient();
                        var disco = await client.GetDiscoveryDocumentAsync(configuration.GetValue<string>("Authority"), cancellationToken);
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
                .AddResilienceHandler("CircuitBreaker", builder =>
                {
                    builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
                    {
                        SamplingDuration = TimeSpan.FromMinutes(1),
                        FailureRatio = 1.0,
                        MinimumThroughput = 2,
                        BreakDuration = TimeSpan.FromMinutes(1)
                    });
                });

            return services;
        }

    }
}
