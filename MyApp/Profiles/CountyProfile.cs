using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Profiles
{
    public class CountyProfile : Profile
    {
        public CountyProfile()
        {
            CreateMap<Entities.County, Models.CountyDto>();
            CreateMap<Models.CountyForCreationDto, Entities.County>();
            CreateMap<Models.CountyForUpdateDto, Entities.County>().ReverseMap();
        }
    }
}
