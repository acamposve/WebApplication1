using AutoMapper;
using DocManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Mappings
{
    public class EmbarquesProfile : Profile
    {
        public static Mapper InitializeAutomapper()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Embarques, Models.Embarques>().ReverseMap();
            });
            config.AssertConfigurationIsValid();
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}