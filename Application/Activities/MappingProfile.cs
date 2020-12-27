using System.Linq;
using AutoMapper;
using Domain;

namespace Application.Activities
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<Activity, ActivityDto>();
      CreateMap<UserActivity, AttendeeDto>()
        // Here I put some configuration for proper showing of nested properties.
        // First argument is a destination member (of AttendeeDto in that case)
        // and to that property, the second argument's value will be assign
        // (AppUser property)
        .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
        .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
        .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(
          x => x.isMain).Url))
        .ForMember(d => d.Following, o => o.MapFrom<FollowingResolver>());
    }
  }
}