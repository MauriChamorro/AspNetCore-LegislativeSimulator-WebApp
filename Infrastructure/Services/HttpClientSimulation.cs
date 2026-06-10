using WebAppMVC.Domain.Models.Projects;
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
        //var stringContent = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);
        Console.WriteLine($"Assigning commissions for {projectId}");
        var response = await _httpClient.GetFromJsonAsync<List<Referral>>($"AssignedCommissions/{projectId}");
        if (response == null)
            Console.WriteLine("The response is null");
        Console.WriteLine(response?[1].Commission.Name);
    }
}