using AutoMapper;
using MyApi.Dtos;
using MyApi.Models;
using System.Linq;

namespace MyApi.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()                
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain == true).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.MapFrom(src => src.DateOfBirth.CalculateUserAge());
                });
            CreateMap<User, UserForDetailListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain == true).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.MapFrom(src => src.DateOfBirth.CalculateUserAge());
                }); 
            CreateMap<Photo, PhotoForUserDto>();
            CreateMap<UserForUpdateDto, User>();
        }
    }
}