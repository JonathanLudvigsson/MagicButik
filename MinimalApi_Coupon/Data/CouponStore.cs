using MinimalApi_Coupon.Models;

namespace MinimalApi_Coupon.Data
{
    public static class CouponStore
    {
        public static List<Coupon> couponList = new List<Coupon>
        { 
            new Coupon {Id = 1, Name = "10OOF", Percent = 10, IsActive = true},
            new Coupon {Id = 2, Name = "20OOF", Percent = 20, IsActive = false},
            new Coupon {Id = 3, Name = "30OOF", Percent = 30, IsActive = false}
        };
    }
}
