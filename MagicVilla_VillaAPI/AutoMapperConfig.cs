using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
namespace MagicVillaNumber_VillaNumberAPI
{
    public class AutoMapperConfig :Profile
    {
        public AutoMapperConfig()
        {

            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<VillaDTOCreate, Villa>().ReverseMap();
            CreateMap<Villa, VillaDTOUpdate>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumberDTOCreate, VillaNumber>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDTOUpdate>().ReverseMap();



        }
    }
}
