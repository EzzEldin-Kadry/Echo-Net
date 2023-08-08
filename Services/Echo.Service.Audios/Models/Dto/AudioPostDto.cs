using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo.Service.Audios.Models.Dto
{
    public class AudioPostDto
    {
        public string? AudioId { get; set; }
        public string? Title { get; set; }
        public string? Description  { get; set; } = null;
        public string? AudioUrl { get; set; }
        public string? OwnerId {get; set;}
        public DateTime PostedDate { get; set; }
    }
}