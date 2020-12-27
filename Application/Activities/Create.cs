using Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistence;
using FluentValidation;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities
{
  public class Create
  {
    // Lack of return item follows the previously mentioned separation of Queries from Commands - Queries should return value,
    // Commands should not.
    public class Command : IRequest
    {
      public Guid Id { get; set; }
      public string Title { get; set; }
      public string Description { get; set; }
      public string Category { get; set; }
      public DateTime Date { get; set; }
      public string City { get; set; }
      public string Venue { get; set; }
    }

    // Validator for data that will go to DB.
    // RuleFor(). has a lot of different options, check them.
    public class CommandValidator : AbstractValidator<Command>
    {
      public CommandValidator()
      {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Venue).NotEmpty();
      }
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

      // MediatR Unit is an empty object that that Handler will return.
      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
      {
        var activity = new Activity
        {
          Id = request.Id,
          Title = request.Title,
          Description = request.Description,
          Category = request.Category,
          Date = request.Date,
          City = request.City,
          Venue = request.Venue
        };

        // From tip of AddAsync - async version should be used only for special value generators - this is not one,
        // so I used the regular one instead.
        _context.Activities.Add(activity);

        // Here I am setting user that creates an event as a host and also and an attending person.
        var user = await _context.Users.SingleOrDefaultAsync(x => 
          x.UserName == _userAccessor.GetCurrentUsername());

        var attendee = new UserActivity
        {
          AppUser = user,
          Activity = activity,
          IsHost = true,
          DateJoined = DateTime.Now
        };

        _context.UserActivities.Add(attendee);

        // SaveChangesAsync() returns a Task of int type - that int is a number of changes saved into db.
        // If the number of changes is bigger than 0, the save has been successful.
        var success = await _context.SaveChangesAsync() > 0;

        // If there was one or more changes to db, then Unit.Value (empty) will be returned to our API.
        // But, returning this via success boolean means that rhe create request was successful, and API controller
        // will return a 200 OK response.
        if (success)
        {
          return Unit.Value;
        }

        // In case of issues, no saves to db, the Exception will be thrown
        throw new Exception("Problem saving changes");
      }
    }
  }
}