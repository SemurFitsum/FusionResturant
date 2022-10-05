﻿using Fusion.Web.Models;
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

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDTOBasedOnLoggedInUser());
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(CartDTO cartDTO)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _cartService.Checkout<ResponseDTO>(cartDTO.CartHeader, accessToken);
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception ex)
            {
                return View(cartDTO);
            }
        }

        [HttpGet]
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
