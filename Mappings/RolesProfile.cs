using AutoMapper;
using DocManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Mappings
{
    public class RolesProfile : Profile
    {
        public static Mapper InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Roles, Models.Roles>().ReverseMap();
            });
            config.AssertConfigurationIsValid();
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}