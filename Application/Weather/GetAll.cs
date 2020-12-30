using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Weather
{
  public class GetAll
  {
    public class Query : IRequest<List<Spot>> { }

    public class Handler : IRequestHandler<Query, List<Spot>>
    {
      private readonly DataContext _context;
      public Handler(DataContext context)
      {
        _context = context;
      }
      public async Task<List<Spot>> Handle(Query request, CancellationToken cancellationToken)
      {
        var spots = await _context.Spots.ToListAsync();

        return spots;
      }
    }
  }
}