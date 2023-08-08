using System.ComponentModel.DataAnnotations;
namespace Echo.Service.Audios.Models
{
    public class AudioPost
    {
        [Key]
        [StringLength(100, ErrorMessage = "")]
        public string? AudioId { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "")]
        public string? Title { get; set; }
        public string? Description  { get; set; } = null;
        [Required]
        [StringLength(1000, ErrorMessage = "")]
        public string? AudioUrl { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "")]
        public string? OwnerId {get; set;}
        public DateTime PostedDate { get; set; }
    }
}