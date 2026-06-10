using System.Net.Mime;
using System.Text;
using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Infrastructure.Services;

public class HttpClientSimulation : ISimulationServices
{
    private readonly HttpClient _httpClient;

    public HttpClientSimulation(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5008/api/Simulation/");
    }

    public async Task AssignCommissionsFor(int projectId)
    {
        var stringContent = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);
        Console.WriteLine($"Assigning commissions for {projectId}");
        var response = await _httpClient.PostAsync($"AssignCommissions/{projectId}", stringContent);
        Console.WriteLine(response.StatusCode);
    }
}