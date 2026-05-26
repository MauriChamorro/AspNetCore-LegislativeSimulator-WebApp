using Microsoft.AspNetCore.Authorization;
using WebAppMVC.Authorization.Requirements;

namespace WebAppMVC.Authorization.Handlers;

public class AddProjectRequirementHandler: AuthorizationHandler<AddProjectRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AddProjectRequirement requirement)
    {
        if(!context.User.HasClaim(c => c.Type == "ProbationDate"))
            return Task.CompletedTask;

        var probationDate = DateTime.Parse(context.User.FindFirst(c => c.Type == "ProbationDate").Value);
        var period = DateTime.Now - probationDate;
        if (period.Days > requirement.DaysRequired)
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}