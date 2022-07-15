﻿using Fusion.Web.Models;

namespace Fusion.Web.Services.IServices
{
    public interface IBaseService:IDisposable
    {
        ResponseDTO responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
