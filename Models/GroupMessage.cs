using System.ComponentModel.DataAnnotations;

namespace Micro_social_platform.Models
{
    public class GroupMessage
    {
        [Key]
        public int? MessageId { get; set; }

        [Required(ErrorMessage = "Message can't be empty")]  
        public string? Content { get; set; }

        public DateTime Date { get; set; }

        public int? GroupId { get; set; }
        public virtual Group? Group { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}
