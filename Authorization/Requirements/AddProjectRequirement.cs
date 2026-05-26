using Microsoft.AspNetCore.Authorization;

namespace WebAppMVC.Authorization.Requirements;

public class AddProjectRequirement : IAuthorizationRequirement
{
    public int DaysRequired { get; }

    public AddProjectRequirement(int daysRequired)
    {
        DaysRequired = daysRequired;
    }
}