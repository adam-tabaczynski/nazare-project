using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
  public class Edit
  {
    public class Command : IRequest
    {
      // User will be able to edit any of these props except the Id
      public Guid Id { get; set; }
      public string Title { get; set; }
      public string Description { get; set; }
      public string Category { get; set; }
      // Added the optional flag due to error showing in handler logic - DateTime cannot be null
      // To bypass that, I made it optional.
      public DateTime? Date { get; set; }
      public string City { get; set; }
      public string Venue { get; set; }
    }
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
    public Handler(DataContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
      var activity = await _context.Activities.FindAsync(request.Id);

       if(activity == null)
      {
        throw new RestException(HttpStatusCode.NotFound,
          new { activity = "Not found"});
      }

      // null coalescing operator - if request.Title is null, then activity.Title will equal to thing on the right side of 
      // double question mark (in that case, to itself - to assure that the default value will persist if user will fail to
      // add new Title)
      activity.Title = request.Title ?? activity.Title;
      activity.Description = request.Description ?? activity.Description;
      activity.Category = request.Category ?? activity.Category;
      // For that to not throw errors, add question mark in model above (optional field sign), normally DateTime cannot be null.
      activity.Date = request.Date ?? activity.Date;
      activity.City = request.City ?? activity.City;
      activity.Venue = request.Venue ?? activity.Venue;


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