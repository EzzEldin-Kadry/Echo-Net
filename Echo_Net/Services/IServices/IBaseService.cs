using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echo_Net.Models;
using Echo_Net.Models.Dto;

namespace Echo_Net.Services.IServices
{
    public interface IBaseService : IDisposable
    {
        ResponseDto ResponseDto { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}