using System.Security.Claims;
using WebAppMVC.Domain.Services;

namespace WebAppMVC.Infrastructure.Services;

public class AccountService: IAccountService
{
    //TODO: get from BD or InMemoryRepo
    public bool ExistUser(string userName, string password) =>
        userName == "admin" && password == "admin" ||
        userName == "legislador" && password == "legislador";

    public List<Claim> GetClaims(string userName)
    {
        //TODO: get from BD or InMemoryRepo
        if (userName == "admin")
        {
            return
            [
                new(ClaimTypes.Name, userName),
                new(ClaimTypes.Country, "ar"),
                new("admin", "true")
            ];
        }

        if (userName == "legislador")
        {
            return
            [
                new(ClaimTypes.Name, userName),
                new(ClaimTypes.Country, "ar"),
                new("Legislador", "true"),
                new("ProbationDate", "2026-5-20")
            ];
        }
        
        return
        [
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Country, "ar")
        ];
    }

    public ClaimsPrincipal GetPrincipal(List<Claim> claims, string schemeName)
    {
        var identity = new ClaimsIdentity(claims, schemeName);
        //user-like
        var principal = new ClaimsPrincipal(identity);
        
        return principal;
    }
}