﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SocialNetwork.Models;
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
        }
    }
}
