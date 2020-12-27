using System;
using System.Collections.Generic;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Domain;
using AutoMapper;
using System.Linq;
using Application.Interfaces;

namespace Application.Activities
{
  public class List
  {

    public class ActivitiesEnvelope
    {
      public List<ActivityDto> Activities { get; set; }
      public int ActivityCount { get; set; }
    }
    // That class allows retrieving a list of activites from DB - no additional parameters needed.
    // List<Activity> is what will be returned.
    // IRequest is a MediatR interface.
    public class Query : IRequest<ActivitiesEnvelope>
    {
      public Query(int? limit, int? offset, bool isGoing, bool isHost, DateTime? startDate)
      {
        Limit = limit;
        Offset = offset;
        IsGoing = isGoing;
        IsHost = isHost;
        StartDate = startDate ?? DateTime.Now;

      }
      public int? Limit { get; set; }
      public int? Offset { get; set; }
      public bool IsGoing { get; set; }
      public bool IsHost { get; set; }
      public DateTime? StartDate { get; set; }
    }

    // I dont have to implement 
    public class Handler : IRequestHandler<Query, ActivitiesEnvelope>
    {
      // I needed DataContext to access DB.
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      private readonly IUserAccessor _userAccessor;
      public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
      {
        _userAccessor = userAccessor;
        _mapper = mapper;
        _context = context;
      }

      // That is a CTRL+. interface implemented from IRequestHandler. That created a Handle method.
      // Query parameter (request) is here for that if I had any properties within Query class, I could access them.
      // cancellationToken - ignore for now.
      // This handler is responsible for getting all of activites.
      public async Task<ActivitiesEnvelope> Handle(Query request, CancellationToken cancellationToken)
      {
        var queryable = _context.Activities
          .Where(x => x.Date >= request.StartDate)
          .OrderBy(x => x.Date)
          .AsQueryable();

        // This will return all the activities that the currently logged user is going to,
        // including the one he is hosting.
        // isGoing and isHost are buttons!!
        if (request.IsGoing && !request.IsHost)
        {
          queryable = queryable.Where(x => x.UserActivities.Any(a => 
            a.AppUser.UserName == _userAccessor.GetCurrentUsername()));
        }

        // That will return just the events the user is hosting.
        if (request.IsHost && !request.IsGoing)
        {
          queryable = queryable.Where(x => x.UserActivities.Any(a => 
            a.AppUser.UserName == _userAccessor.GetCurrentUsername() && a.IsHost));          
        }

        var activities = await queryable
          .Skip(request.Offset ?? 0)
          .Take(request.Limit ?? 3)
          .ToListAsync();

        return new ActivitiesEnvelope
        {
          Activities = _mapper.Map<List<Activity>, List<ActivityDto>>(activities),
          ActivityCount = queryable.Count()
        };
      }
    }
  }
}