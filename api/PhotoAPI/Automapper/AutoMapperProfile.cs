using AutoMapper;
using PhotoAPI.Models.Dto;
using PhotoAPI.Models.Entity;


namespace PhotoAPI.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Photo, PhotoDTO>();
        }
    }
}
