using Fusion.Web.Models;
using Fusion.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fusion.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;
        public CartController(IProductService productService, ICartService cartService,ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDTOBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO1)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDTO>(userId, accessToken);

            CartDTO cartDTO = new();
            if (response != null && response.IsSuccess)
            {
                cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
            }

            if (cartDTO.CartHeader != null)
            {
                foreach (var detail in cartDTO.CartDetails)
                {
                    cartDTO.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }
            }

            cartDTO.CartHeader.CouponCode = cartDTO1.CartHeader.CouponCode;

            var response2 = await _cartService.ApplyCoupon<ResponseDTO>(cartDTO, accessToken);

            if (response2 != null && response2.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveCoupon<ResponseDTO>(cartDTO.CartHeader.UserId, accessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        public async Task<IActionResult> Remove(int CartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveFromCartAsync<ResponseDTO>(CartDetailsId, accessToken);
                        
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDTOBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDTO cartDTO)
        {
            try
            {
                var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _cartService.GetCartByUserIdAsync<ResponseDTO>(userId, accessToken);

                CheckoutDTO cartDTO1 = new();
                if (response != null && response.IsSuccess)
                {
                    cartDTO1 = JsonConvert.DeserializeObject<CheckoutDTO>(Convert.ToString(response.Result));

                    cartDTO1.CartHeaderId = cartDTO.CartHeader.CartHeaderId;
                    cartDTO1.UserId = cartDTO.CartHeader.UserId;
                    cartDTO1.CouponCode = cartDTO.CartHeader.CouponCode;
                    cartDTO1.OrderTotal = cartDTO.CartHeader.OrderTotal;
                    cartDTO1.DiscountTotal = cartDTO.CartHeader.DiscountTotal;
                    cartDTO1.FirstName = cartDTO.CartHeader.FirstName;
                    cartDTO1.LastName = cartDTO.CartHeader.LastName;
                    cartDTO1.PickupDateTime = cartDTO.CartHeader.PickupDateTime;
                    cartDTO1.Phone = cartDTO.CartHeader.Phone;
                    cartDTO1.Email = cartDTO.CartHeader.Email;
                    cartDTO1.CardNumber = cartDTO.CartHeader.CardNumber;
                    cartDTO1.CVV = cartDTO.CartHeader.CVV;
                    cartDTO1.ExpiryMonthYear = cartDTO.CartHeader.ExpiryMonthYear;
                    
                }               

                var response1 = await _cartService.Checkout<ResponseDTO>(cartDTO1, accessToken);

                if (!response1.IsSuccess)
                {
                    TempData["Error"] = response1.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }

                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception ex)
            {
                return View(cartDTO);
            }
        }

        public async Task<IActionResult> Confirmation()
        {
            return View();
        }
        private async Task<CartDTO> LoadCartDTOBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDTO>(userId, accessToken);

            CartDTO cartDTO = new();
            if (response != null && response.IsSuccess)
            {
                cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
            }

            if (cartDTO.CartHeader!=null)
            {
                if (!string.IsNullOrEmpty(cartDTO.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon<ResponseDTO>(cartDTO.CartHeader.CouponCode, accessToken);

                    if (coupon != null && coupon.IsSuccess)
                    {
                        var couponObj = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(coupon.Result));
                        cartDTO.CartHeader.DiscountTotal = couponObj.DiscountAmount;
                    }
                }

                foreach (var detail in cartDTO.CartDetails) 
                {
                    cartDTO.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }

                cartDTO.CartHeader.OrderTotal -= cartDTO.CartHeader.DiscountTotal;
            }
            return cartDTO;
        }
    }
}
