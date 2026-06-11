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

    public async Task<string> AssignCommissionsFor(int projectId)
    {
        if (_httpContextAccessor.HttpContext == null)
            return "Hubo un error en el pedido";
        if (!_httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
            return "El usuario debe estar autenticado";
        if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(ExpedientesAuthValues.CookieName, out var cookie))
            return "Hubo un error en la autenticación del usuario";

        _httpClient.DefaultRequestHeaders.Add("Cookie", $"{ExpedientesAuthValues.CookieName}={cookie}");
        var content = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _httpClient.PostAsync($"AssignCommissions/{projectId}", content);
        var result = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
            result = "Comisiones Asignadas";
        return result;
    }
}