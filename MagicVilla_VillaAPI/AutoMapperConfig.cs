using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using System.Runtime.CompilerServices;

namespace MagicVilla_VillaAPI
{
    public class AutoMapperConfig :Profile
    {
        public AutoMapperConfig()
        {

            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<VillaDTOCreate, Villa>().ReverseMap();
            CreateMap<Villa, VillaDTOUpdate>().ReverseMap();



        }
    }
}
