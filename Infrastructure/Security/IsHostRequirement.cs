using System;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Infrastructure.Security
{
  public class IsHostRequirement : IAuthorizationRequirement
  {
  }

  // Goal of that class is to establish if user is a host of an activity.
  // Idea is - get the user ID, get the activity ID, get the host ID of activity,
  // check if user ID equals host ID.
  public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataContext _context;
    public IsHostRequirementHandler(IHttpContextAccessor httpContextAccessor,

    DataContext context)
    {
      _context = context;
      _httpContextAccessor = httpContextAccessor;
    }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
    IsHostRequirement requirement)
  {
    var currentUserName = _httpContextAccessor.HttpContext.User?.Claims?
      .SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

    // ID that will be passed is in form of string, thats why I transformed it
    // from guid.
    var activityId = Guid.Parse(_httpContextAccessor.HttpContext.Request.RouteValues
      .SingleOrDefault(x => x.Key == "id").Value.ToString());

    // Here I get an activity for the given activity id.
    var activity = _context.Activities.FindAsync(activityId).Result;

    // Gets the host of activity (isHost will be true only for one user).
    var host = activity.UserActivities.FirstOrDefault(x => x.IsHost);

    // if host is the user currently logged, then the Succeed property of first
    // argument sets the requirement (second argument) as succeeded.
    // If user is not the host then the app will return 403 forbidden.
    if (host?.AppUser?.UserName == currentUserName)
    {
      context.Succeed(requirement);
    }

    return Task.CompletedTask;
  }
}
}