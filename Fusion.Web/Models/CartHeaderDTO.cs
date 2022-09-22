namespace Fusion.Web.Models
{
    public class CartHeaderDTO
    {
        public int CareHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotal { get; set; }
    }
}
