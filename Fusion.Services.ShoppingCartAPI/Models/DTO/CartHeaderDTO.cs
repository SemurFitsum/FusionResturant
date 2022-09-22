using System.ComponentModel.DataAnnotations;

namespace Fusion.Services.ShoppingCartAPI.Models.DTO
{
    public class CartHeaderDTO
    {
        public int CareHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
    }
}
