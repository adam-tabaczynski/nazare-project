using System.Reflection.Metadata;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using MediatR;
using Newtonsoft.Json;
using Persistence;
using static Application.Weather.WeatherModel;

namespace Application.Weather
{
  public class GetSpot
  {
    public class Query : IRequest<Spot>
    {
      public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Spot>
    {
      private readonly DataContext _context;
      public Handler(DataContext context)
      {
        _context = context;
      }
      public async Task<Spot> Handle(Query request, CancellationToken cancellationToken)
      {
        var spot = await _context.Spots.FindAsync(request.Id);

        if (spot == null)
        {
          throw new RestException(HttpStatusCode.NotFound,
            new { spot = "Not found" });
        }

        return spot;
      }
    }
  }
}