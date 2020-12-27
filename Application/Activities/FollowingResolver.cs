using System.Linq;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
  //1arg - from what I take data, 2arg - to what I want to put it, 3arg - type of variable the
  // value of I want to resolve.
  public class FollowingResolver : IValueResolver<UserActivity, AttendeeDto, bool>
  {
    private readonly DataContext _context;
    private readonly IUserAccessor _userAccessor;
    public FollowingResolver(DataContext context, IUserAccessor userAccessor)
    {
      _userAccessor = userAccessor;
      _context = context;
    }

    // Cant make it async, because I can only return boolean from it, not Task<boolean>
    public bool Resolve(UserActivity source, AttendeeDto destination, bool destMember, ResolutionContext context)
    {
      // Added .Result to be able to use SingleOrDefaultAsync method.
      var currentUser = _context.Users.SingleOrDefaultAsync(x => x.UserName ==
        _userAccessor.GetCurrentUsername()).Result;

      if (currentUser.Followings.Any(x => x.TargetId == source.AppUserId))
      {
        return true;
      }
      return false;
    }
  }
}