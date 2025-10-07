using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoleManagementApi.Services
{
    public class LinkCheckerService : ILinkCheckerService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LinkCheckerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> CheckLinkStatusAsync(string urlOrPath, string type)
        {
            if (string.IsNullOrWhiteSpace(urlOrPath))
                return "Broken";

            if (type == "File")
            {
                var fileName = Path.GetFileName(urlOrPath);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
                return File.Exists(fullPath) ? "Valid" : "Broken";
            }
            else
            {
                if (!Uri.TryCreate(urlOrPath, UriKind.Absolute, out var uri))
                    return "Broken";

                try
                {
                    var client = new HttpClient()
                    {
                        Timeout = TimeSpan.FromSeconds(10)
                    };

                    var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));

                    // fallback to GET if HEAD not allowed
                    if (response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
                        response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, uri));

                    return response.IsSuccessStatusCode ? "Valid" : "Broken";
                }
                catch
                {
                    return "Broken";
                }
            }
        }
    }
}
