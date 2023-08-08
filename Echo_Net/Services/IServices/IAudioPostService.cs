using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echo_Net.Models.Dto;

namespace Echo_Net.Services
{
    public interface IAudioPostService
    {
        Task<T> GetAllAudioPostsAsync<T>();
        Task<T> GetAudioPostByIdAsync<T>(string id);
        Task<T> GetAudioPostsByOwnerIdAsync<T>(string ownerId);
        Task<T> CreateAudioPostAsync<T>(AudioPostDto audioPostDto);
        Task<T> UpdateAudioPostAsync<T>(AudioPostDto audioPostDto);
        Task<T> DeleteAudioPostAsync<T>(string id);
    }
}