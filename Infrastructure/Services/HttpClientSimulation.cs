using System.Net.Mime;
using System.Text;
using WebAppMVC.Authorization;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Infrastructure.Services;

public class HttpClientSimulation : ISimulationServices
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientSimulation(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _httpClient.BaseAddress = new Uri("http://localhost:5008/api/Simulation/");
    }

    public async Task AssignCommissionsFor(int projectId)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(ExpedientesAuthValues.CookieName, out var cookie))
            {
                _httpClient.DefaultRequestHeaders.Add("Cookie", $"{ExpedientesAuthValues.CookieName}={cookie}");
                var content = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);
                Console.WriteLine($"Assigning commissions for {projectId}");
                var response = await _httpClient.PostAsync($"AssignCommissions/{projectId}", content);
                Console.WriteLine(response.StatusCode);   
                Console.WriteLine(await response.Content.ReadAsStringAsync());   
            }
        }
    }
}