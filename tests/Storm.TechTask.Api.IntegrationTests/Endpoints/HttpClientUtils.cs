using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Storm.TechTask.Api.IntegrationTests.Endpoints
{
    public static class HttpClientUtils
    {
        public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string url, object payload)
        {
            return await httpClient.PostAsync(url, NewHttpContent(payload));
        }

        private static HttpContent? NewHttpContent(object content)
        {
            HttpContent? httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        private static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var jtw = new Utf8JsonWriter(stream))
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                jsonOptions.Converters.Add(new JsonStringEnumConverter());
                JsonSerializer.Serialize(jtw, value, jsonOptions);
                jtw.Flush();
            }
        }
    }
}
