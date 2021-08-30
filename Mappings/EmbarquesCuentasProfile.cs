using AutoMapper;
using DocManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Mappings
{
    public class EmbarquesCuentasProfile : Profile
    {
        public static Mapper InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmbarquesCuentas, Models.EmbarquesCuentas>().ReverseMap();
            });
            config.AssertConfigurationIsValid();
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}