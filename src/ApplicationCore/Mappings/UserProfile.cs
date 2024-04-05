using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.DTOs.User;
using Domain.Entities;

namespace ApplicationCore.Mappings
{
    
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserTDO, Usuarios>()
                .ForMember(x => x.IDusuario, y => y.Ignore());
        }

    }
}
