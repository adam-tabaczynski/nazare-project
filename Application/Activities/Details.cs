using System.Net;
using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;
using Application.Errors;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.Activities
{
  public class Details
  {
    // Similiar to List.cs - Query is a class required for MediatR, here only one Activity will be returned
    public class Query : IRequest<ActivityDto>
    {
      public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, ActivityDto>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      public Handler(DataContext context, IMapper mapper)
      {
        _mapper = mapper;
        _context = context;
      }

      // As in List.cs - this handler finds a single activity for given id (here, guid)
      public async Task<ActivityDto> Handle(Query request, CancellationToken cancellationToken)
      {

        var activity = await _context.Activities
          .FindAsync(request.Id);

          // // These has been commented out - they were working for Eager Loading,
          // // but now I use Lazy Loading.
          // .Include(x => x.UserActivities)
          // .ThenInclude(x => x.AppUser)
          // .SingleOrDefaultAsync(x => x.Id == request.Id);

        if (activity == null)
        {
          throw new RestException(HttpStatusCode.NotFound,
            new { activity = "Not found" });
        }

        var activityToReturn = _mapper.Map<Activity, ActivityDto>(activity);

        return activityToReturn;
      }
    }
  }
}