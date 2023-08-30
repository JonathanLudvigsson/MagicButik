namespace MinimalApi_Coupon.Models.DTOs
{
    public class CouponCreateDTO
    {
        public string Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
    }
}
