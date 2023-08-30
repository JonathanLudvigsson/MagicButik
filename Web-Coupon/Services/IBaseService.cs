using Web_Coupon.Models;

namespace Web_Coupon.Services
{
    public interface IBaseService :IDisposable
    {
        ResponseDto responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);

    }
}
