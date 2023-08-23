using System.Net;

namespace MinimalApi_Coupon.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<String>();
        }

        public bool IsSuccess { get; set; }
        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
