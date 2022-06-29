using System.Net;
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;


namespace Storm.TechTask.Web.ApiClient
{
    public static partial class ApiClientServiceCollectionExtensions
    {
        public class ApiClientHttpClientHandler : DelegatingHandler
        {
            private readonly IHttpContextAccessor httpContextAccessor;

            public ApiClientHttpClientHandler(IHttpContextAccessor httpContextAccessor)
            {
                this.httpContextAccessor = httpContextAccessor;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                // Maybe we are using auth code or hybrid?
                var auth = request.Headers.Authorization;
                if (auth == null)
                {
                    var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            break;
                        case HttpStatusCode.BadRequest:
                            throw new ApiClientException(
                                System.Text.Json.JsonSerializer.Deserialize<ValidationProblemDetails>(errorResponse));
                        case HttpStatusCode.Unauthorized:
                        case HttpStatusCode.Forbidden:
                        default:
                            throw new ApiClientException(
                               System.Text.Json.JsonSerializer.Deserialize<ProblemDetails>(errorResponse));
                    }
                }

                return response;
            }
        }

    }
}
