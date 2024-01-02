using API.Dtos;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles :Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                  .ForMember(dest => dest.PhotoUrl, op => op.MapFrom(src => src.Photos.FirstOrDefault().Url))
                  .ForMember(dest => dest.Age, op => op.MapFrom(src => src.BirthOfDate.CalculateAge()));
            //.ForMember(dest=>dest.PhotoUrl,opt=>opt.MapFrom(src=>src.Photos.FirstOrDefault().Url));  
            CreateMap<Photo, PhotoDto>();  
            CreateMap<UpdateUserDto, AppUser>().ReverseMap();
        }
    }
}
