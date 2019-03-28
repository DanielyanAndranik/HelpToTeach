using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HelpToTeach.Data.Models;
using HelpToTeach.Data.Transfer;

namespace WebApplication.Helpers
{
    public class AutoMapperUser : Profile
    {
        public AutoMapperUser()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
