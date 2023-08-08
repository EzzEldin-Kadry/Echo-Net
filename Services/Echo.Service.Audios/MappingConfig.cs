using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Echo.Service.Audios.Models;
using Echo.Service.Audios.Models.Dto;

namespace Echo.Service.Audios
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => 
            {
                config.CreateMap<AudioPostDto, AudioPost>();
                config.CreateMap<AudioPost, AudioPostDto>();    
            });
            return mappingConfig;
        }
    }
}