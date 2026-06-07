using System.Security.Claims;

namespace WebAppMVC.Infrastructure.Interfaces;

public interface IAccountService
{
    bool ExistUser(string userName, string password);
    List<Claim> GetClaims(string userName);
    ClaimsPrincipal GetPrincipal(List<Claim> claims, string schemeName);
}