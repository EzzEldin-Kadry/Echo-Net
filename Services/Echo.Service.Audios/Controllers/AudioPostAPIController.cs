using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Echo.Service.Audios.Models.Dto;
using Echo.Service.Audios.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Echo.Service.Audios.Controllers
{
    [ApiController]
    [Route("api/audioPost")]
    public class AudioPostAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IAudioPostRepository _audioPostRepository;
        public AudioPostAPIController(IAudioPostRepository audioPostRepository)
        {
            _audioPostRepository = audioPostRepository;
            _response = new ResponseDto();
        }
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                IEnumerable<AudioPostDto> audioPostDtos = await _audioPostRepository.GetAudioPosts();
                _response.Result = audioPostDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("audio/{audioId}")]
        public async Task<object> GetById(string audioId)
        {
            try
            {
                AudioPostDto audioPostDto = await _audioPostRepository.GetAudioPostById(audioId);
                _response.Result = audioPostDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("owner/{ownerId}")]
        public async Task<object> GetAudioPostsByOwnerId(string ownerId)
        {
            try
            {
                IEnumerable<AudioPostDto> audioPostDto = await _audioPostRepository.GetAudioPostsOfOwner(ownerId);
                _response.Result = audioPostDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPost]
        public async Task<object> Post([FromBody] AudioPostDto audioPostDto)
        {
            try
            {
                _response.Result = await _audioPostRepository.CreateUpdateAudioPost(audioPostDto);  
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPut]
        public async Task<object> Put([FromBody] AudioPostDto audioPostDto)
        {
            try
            {
                _response.Result = await _audioPostRepository.CreateUpdateAudioPost(audioPostDto, true);  
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpDelete]
        public async Task<object> Delete(string audioId)
        {
            try
            {
                _response.Result = await _audioPostRepository.DeleteAudio(audioId);  
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}