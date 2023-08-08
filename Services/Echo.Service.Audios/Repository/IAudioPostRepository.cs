using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echo.Service.Audios.Models.Dto;

namespace Echo.Service.Audios.Repository
{
    public interface IAudioPostRepository
    {
        Task<AudioPostDto> GetAudioPostById(string audioId);
        Task<AudioPostDto> CreateUpdateAudioPost(AudioPostDto audioPostDto, bool isUpdate = false);
        Task<bool> DeleteAudio(string audioId);
        Task<IEnumerable<AudioPostDto>> GetAudioPosts();
        Task<IEnumerable<AudioPostDto>> GetAudioPostsOfOwner(string ownerId);
    }
}