using AutoMapper;
using MinimalApi_Coupon.Models;
using MinimalApi_Coupon.Models.DTOs;

namespace MinimalApi_Coupon
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();
        }
    }
}
