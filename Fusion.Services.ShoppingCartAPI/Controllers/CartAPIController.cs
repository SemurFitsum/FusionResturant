﻿using AzureMessageBus;
using Fusion.Services.ShoppingCartAPI.Messages;
using Fusion.Services.ShoppingCartAPI.Models.DTO;
using Fusion.Services.ShoppingCartAPI.Repository;
using Fusion.ShoppingCartAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Fusion.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMessageBus _messageBus;
        protected ResponseDTO _response;
        public CartAPIController(ICartRepository cartRepository, IMessageBus messageBus, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _messageBus = messageBus;
            this._response = new ResponseDTO();            
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDTO cartDTO = await _cartRepository.GetCartByUserId(userId);
                _response.Result = cartDTO;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDTO cartDTO)
        {
            try
            {
                CartDTO cartDT = await _cartRepository.CreateUpdateCart(cartDTO);
                _response.Result = cartDT;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDTO cartDTO)
        {
            try
            {
                CartDTO cartDT = await _cartRepository.CreateUpdateCart(cartDTO);
                _response.Result = cartDT;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody]int cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveFromCart(cartId);
                _response.Result = isSuccess;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                bool isSuccess = await _cartRepository.ApplyCoupon(cartDTO.CartHeader.UserId,
                    cartDTO.CartHeader.CouponCode);
                _response.Result = isSuccess;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userID)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveCoupon(userID);
                _response.Result = isSuccess;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDTO checkoutHeader)
        {
            try
            {
                CartDTO cartDTO = await _cartRepository.GetCartByUserId(checkoutHeader.UserId);

                if (cartDTO == null)
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
                {
                    CouponDTO coupon = await _couponRepository.GetCoupon(checkoutHeader.CouponCode);
                    if (checkoutHeader.DiscountTotal != coupon.DiscountAmount)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessage = new List<string>() { "Coupon Price has changed, please confirm" };
                        _response.DisplayMessage = "Coupon Price has changed, please confirm";
                        return _response;
                    }
                }

                checkoutHeader.cartDetails = cartDTO.CartDetails;
                // logic to add message to process order.
                //await _messageBus.PublisheMessage(checkoutHeader, "checkoutmessagetopic");
                await _messageBus.PublisheMessage(checkoutHeader, "checkoutqueue");

                await _cartRepository.ClearCart(checkoutHeader.UserId);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
