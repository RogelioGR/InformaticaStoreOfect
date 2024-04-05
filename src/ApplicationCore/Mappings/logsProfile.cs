using ApplicationCore.DTOs;
using ApplicationCore.DTOs.User;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Mappings
{
    public class logsProfile : Profile
    {
        public logsProfile()
        {
            CreateMap<logsDTO, logs>()
                .ForMember(x => x.id_logs, y => y.Ignore());
        }
    }
}
