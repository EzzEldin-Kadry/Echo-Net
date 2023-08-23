using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Echo_Net.Models.Dto
{
    public class AudioPostDto
    {
        public string? AudioId { get; set; }
        public string? Title { get; set; }
        public string? Description  { get; set; } = null;
        public string? AudioUrl { get; set; }
        public string? OwnerId {get; set;}
        public DateTime PostedDate { get; set; }
        public AudioPostDto(string? title, string? description, string? audioUrl)
        {
            AudioId = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            AudioUrl = audioUrl;
            OwnerId = Guid.NewGuid().ToString();
            PostedDate = DateTime.Now;
        }
        public static string ConstructFilePath(string fileName)
        {
            string filePath = Path.Combine(Constants.EchoesLocation, 
                    fileName + Constants.AudioExtension);
            return filePath;
        }
    }
}