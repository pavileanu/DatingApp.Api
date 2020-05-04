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
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDbo, Photo>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageReturnDto>()
                .ForMember(m => m.SenderPhotoUrl, opt => opt
                    .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.isMain).Url))
                .ForMember(m => m.RecipientPhotoUrl, opt => opt
                    .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.isMain).Url));

        }
    }
}