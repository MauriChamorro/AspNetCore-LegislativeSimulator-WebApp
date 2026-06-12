using System.Net;
using System.Net.Mime;
using System.Text;
using WebAppMVC.Authorization;
using WebAppMVC.Domain.Models.Projects;
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

    private async Task<HttpResponseMessage> SendPost(string url, ByteArrayContent? content = null)
    {
        var messageValidation = RequestValidations();
        if (messageValidation != null)
        {
            var badRequest = new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest);
            badRequest.Content = StringContent(messageValidation);
            return  badRequest;
        }

        var cookie = _httpContextAccessor.HttpContext!.Request.Cookies[ExpedientesAuthValues.CookieName];
        _httpClient.DefaultRequestHeaders.Add("Cookie", $"{ExpedientesAuthValues.CookieName}={cookie}");
        content ??= EmptyContent();
        return await _httpClient.PostAsync(url, content);
    }

    private ByteArrayContent EmptyContent() =>
        new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);

    private ByteArrayContent StringContent(string contentAsString) =>
        new StringContent(contentAsString, Encoding.UTF8, MediaTypeNames.Application.Json);

    public async Task<string> AssignCommissionsFor(int projectId)
    {
        var url = $"AssignCommissions/{projectId}";
        var response = await SendPost(url);
        var resultContent = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
            resultContent = "Comisiones Asignadas";
        return resultContent;
    }

    public async Task<string> DoNextReferringRandomly(int projectId)
    {
        var validationResult = RequestValidations();
        if (validationResult != null)
            return validationResult;

        var cookie = _httpContextAccessor.HttpContext!.Request.Cookies[ExpedientesAuthValues.CookieName];
        _httpClient.DefaultRequestHeaders.Add("Cookie", $"{ExpedientesAuthValues.CookieName}={cookie}");
        var content = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _httpClient.PostAsync($"DoNextReferringRandomly/{projectId}", content);
        var result = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
            result = "Giro aleatorio realizado";
        return result;
    }

    public async Task<string> EditReferral(int projectId, int commissionId, ReferralState state)
    {
        var validationResult = RequestValidations();
        if (validationResult != null)
            return validationResult;

        var cookie = _httpContextAccessor.HttpContext!.Request.Cookies[ExpedientesAuthValues.CookieName];
        _httpClient.DefaultRequestHeaders.Add("Cookie", $"{ExpedientesAuthValues.CookieName}={cookie}");
        var content = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);
        var response =
            await _httpClient.PostAsync(
                $"EditReferral/{projectId}?commissionId={commissionId}&referralState={(int)state}", content);
        var result = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
            result = "Giro guardado correctamente";
        return result;
    }

    public async Task<string> DoSessionRandomly(int projectId)
    {
        var validationResult = RequestValidations();
        if (validationResult != null)
            return validationResult;

        var cookie = _httpContextAccessor.HttpContext!.Request.Cookies[ExpedientesAuthValues.CookieName];
        _httpClient.DefaultRequestHeaders.Add("Cookie", $"{ExpedientesAuthValues.CookieName}={cookie}");
        var content = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _httpClient.PostAsync($"DoSessionRandomly/{projectId}", content);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    public async Task<string> DoSession(int projectId, int sessionResult)
    {
        var messageValidation = RequestValidations();
        if (messageValidation != null)
            return messageValidation;

        var cookie = _httpContextAccessor.HttpContext!.Request.Cookies[ExpedientesAuthValues.CookieName];
        _httpClient.DefaultRequestHeaders.Add("Cookie", $"{ExpedientesAuthValues.CookieName}={cookie}");
        var content = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await _httpClient.PostAsync($"DoSession/{projectId}?result={sessionResult}", content);
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    private string? RequestValidations()
    {
        if (_httpContextAccessor.HttpContext == null)
            return "Hubo un error en el pedido";
        if (!_httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
            return "El usuario debe estar autenticado";
        if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(ExpedientesAuthValues.CookieName, out _))
            return "Hubo un error en la autenticación del usuario";
        return null;
    }
}