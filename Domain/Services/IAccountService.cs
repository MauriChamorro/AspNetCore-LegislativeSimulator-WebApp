using System.Security.Claims;

namespace WebAppMVC.Domain.Services;

public interface IAccountService
{
    bool ExistUser(string userName, string password);
    List<Claim> GetClaims(string userName);
    ClaimsPrincipal GetPrincipal(List<Claim> claims, string schemeName);
}