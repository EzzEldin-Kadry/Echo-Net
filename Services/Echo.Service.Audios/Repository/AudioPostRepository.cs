using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Echo.Service.Audios.Models;
using Echo.Service.Audios.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Echo.Service.Audios.Repository
{
    public class AudioPostRepository : IAudioPostRepository
    {
        //add applicationdbcontext and mapper
        private readonly ApplicationDbContent _db;
        private IMapper _mapper;
        public AudioPostRepository(ApplicationDbContent db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<AudioPostDto> CreateUpdateAudioPost(AudioPostDto audioPostDto, bool isUpdate = false)
        {
            AudioPost audioPost = _mapper.Map<AudioPostDto, AudioPost>(audioPostDto);
            if (isUpdate)
            {
                _db.AudioPosts.Update(audioPost);
            }
            else
            {
                _db.AudioPosts.Add(audioPost);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<AudioPost, AudioPostDto>(audioPost);
        }

        public async Task<bool> DeleteAudio(string audioId)
        {
            try
            {
                AudioPost? audioPost = await _db.AudioPosts.FirstOrDefaultAsync(x => x.AudioId == audioId);
                if (audioPost is null)
                {
                    return false;
                }
                _db.AudioPosts.Remove(audioPost);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AudioPostDto> GetAudioPostById(string audioId)
        {
            AudioPost? audioPost = await _db.AudioPosts.Where(x => x.AudioId == audioId).FirstOrDefaultAsync();
            return _mapper.Map<AudioPostDto>(audioPost);
        }

        public async Task<IEnumerable<AudioPostDto>> GetAudioPosts()
        {
            List<AudioPost> audioPostList = await _db.AudioPosts.ToListAsync();
            return _mapper.Map<List<AudioPostDto>>(audioPostList);
        }

        public async Task<IEnumerable<AudioPostDto>> GetAudioPostsOfOwner(string ownerId)
        {
            List<AudioPost> audioPostsList = await _db.AudioPosts.Where(x => x.OwnerId == ownerId).ToListAsync();
            return _mapper.Map<List<AudioPostDto>>(audioPostsList);
        }
    }
}