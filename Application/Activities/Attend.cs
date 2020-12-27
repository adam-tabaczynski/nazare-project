using System.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Application.Activities
{
  public class Attend
  {
    public class Command : IRequest
    {
      public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
      private readonly DataContext _context;
      private readonly IUserAccessor _userAccessor;
      public Handler(DataContext context, IUserAccessor userAccessor)
      {
        _userAccessor = userAccessor;
        _context = context;
      }

      // Handler of adding new attendees to event
      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        
        // Takes an activity from db.
        var activity = await _context.Activities.FindAsync(request.Id);

        if (activity == null)
        {
          throw new RestException
            (HttpStatusCode.NotFound, new {Activity = "Could not find activity"});
        }

        // gets the current user.
        var user = await _context.Users.SingleOrDefaultAsync(x => 
          x.UserName == _userAccessor.GetCurrentUsername());

        // checks if there is a record of user's Id and chosen activity's Id - if not,
        // all is well. If there is, then it means that the user already attends that
        // activity.
        var attendance = await _context.UserActivities.SingleOrDefaultAsync(x =>
          x.ActivityId == activity.Id && x.AppUserId == user.Id);

        if (attendance != null)
        {
          throw new RestException(HttpStatusCode.BadRequest, new {Attendance = 
            "Already attending this activity"});
        }

        // In case of null value of attendance, the user is being assigned to chosen activity.
        attendance = new UserActivity
        {
          Activity = activity,
          AppUser = user,
          IsHost = false,
          DateJoined = DateTime.Now
        };

        // EFCore is smart, it takes activityId and and AppUserId and saves it to db.
        _context.UserActivities.Add(attendance);

        var success = await _context.SaveChangesAsync() > 0;

        if (success)
        {
          return Unit.Value;
        }

        throw new Exception("Problem saving changes");
      }
    }
  }
}