using AutoMapper;
using AdvertAPI.Models;
using WebAdvert.WebAPI.Models;

namespace WebAdvert.WebAPI.Services
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile() {
            CreateMap<AdvertModel, AdvertDbModel>();
        }  
    }
}
