using EmployeeManagementSystem.Constants;
using EmployeeManagementSystem.Models.Auth;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EmployeeManagementSystem.Network
{
    public class AuthNetworkService
    {
        private readonly HttpClient _httpClient;

        public AuthNetworkService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> RegisterUserAsync(RegisterViewModel model, string firebaseToken)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firebaseToken);

            return await _httpClient.PostAsync(ApiRoutes.Register, content);
        }
    }
}