using WebAppMVC.Infrastructure.Interfaces;

namespace WebAppMVC.Infrastructure.Services;

public class HttpClientSimulation : ISimulationServices
{
    private readonly HttpClient _httpClient;

    public HttpClientSimulation(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:5008/api/Simulation");
    }

    public void AssignCommissionsFor(int projectId)
    {
        Console.WriteLine($"Assigning commissions for {projectId}");
    }
}