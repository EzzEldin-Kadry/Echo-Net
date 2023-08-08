using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echo_Net.Models;
using Echo_Net.Models.Dto;

namespace Echo_Net.Services
{
    public class AudioPostService : BaseService, IAudioPostService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public AudioPostService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> CreateAudioPostAsync<T>(AudioPostDto audioPostDto)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = Constants.APIType.POST,
                Data = audioPostDto,
                Url = Constants.AudioPostBase + "/api/audioPost",
                AccessToken = ""
            });
        }

        public async Task<T> DeleteAudioPostAsync<T>(string id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = Constants.APIType.DELETE,
                Url = Constants.AudioPostBase + "/api/audioPost/" + id,
                AccessToken = ""
            });
        }

        public async Task<T> GetAllAudioPostsAsync<T>()
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = Constants.APIType.GET,
                Url = Constants.AudioPostBase + "/api/audioPost",
                AccessToken = ""
            });
        }

        public async Task<T> GetAudioPostByIdAsync<T>(string id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = Constants.APIType.GET,
                Url = Constants.AudioPostBase + "/api/audioPost/audio/" + id,
                AccessToken = ""
            });
        }

        public async Task<T> GetAudioPostsByOwnerIdAsync<T>(string ownerId)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = Constants.APIType.GET,
                Url = Constants.AudioPostBase + "/api/audioPost/owner/" + ownerId,
                AccessToken = ""
            });
        }

        public async Task<T> UpdateAudioPostAsync<T>(AudioPostDto audioPostDto)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = Constants.APIType.PUT,
                Data = audioPostDto,
                Url = Constants.AudioPostBase + "/api/audioPost",
                AccessToken = ""
            });
        }
    }
}