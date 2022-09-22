using System.ComponentModel.DataAnnotations;

namespace Fusion.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CareHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
    }
}
