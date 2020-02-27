using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Profiles
{
    public class StateProfile : Profile
    {
        public StateProfile()
        {
            CreateMap<Entities.State, Models.StateWithoutCountiesDto>();
            CreateMap<Entities.State, Models.StateDto>();
        }
    }
}
