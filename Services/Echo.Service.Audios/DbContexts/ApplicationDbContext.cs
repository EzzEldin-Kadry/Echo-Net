using Microsoft.EntityFrameworkCore;
using Echo.Service.Audios.Models;
namespace Echo.Service.Audios
{
    public class ApplicationDbContent : DbContext
    {
        public ApplicationDbContent(DbContextOptions<ApplicationDbContent> options) : base(options)
        {
            
        }
        public DbSet<AudioPost> AudioPosts { get; set; }
    }
}