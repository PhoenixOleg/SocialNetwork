using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Models.Users;
using SocialNetwork.ViewModels.Account;

namespace SocialNetwork
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<RegisterViewModel, User>()
                .ForMember (m => m.Email, opt => opt.MapFrom(src => src.EmailReg))
                .ForMember(m => m.UserName, opt => opt.MapFrom (src => src.Login))
                .ForMember(m => m.BirthDate, opt => opt.MapFrom(src => new DateTime((int)src.Year, (int)src.Month, (int)src.Day)))
                ;
            CreateMap<LoginViewModel, User>();
            
            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserEditViewModel>()
                .ForMember(m => m.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(m => m.Login, opt => opt.MapFrom(src => src.UserName));

            CreateMap<UserWithFriendExt, User>();
            CreateMap<User, UserWithFriendExt>();
        }
    }
}
