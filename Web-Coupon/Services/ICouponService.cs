using MinimalApi_Coupon.Models.DTOs;

namespace Web_Coupon.Services
{
    public interface ICouponService
    {
        Task<T> GetAllCoupons<T>();
        Task<T> GetCoupon<T>(int id);
        Task<T> CreateCoupon<T>(CouponDTO couponDTO);
        Task<T> UpdateCoupon<T>(CouponDTO couponDTO);
        Task<T> DeleteCoupon<T>(int id);
    }
}
