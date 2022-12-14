using System.ComponentModel.DataAnnotations;

namespace Micro_social_platform.Models
{
    public class Article
    {
            [Key]
            public int Id { get; set; }
            [Required(ErrorMessage = "Titlul este obligatoriu")]
            public string Title { get; set; }
            [Required(ErrorMessage = "Continutul articolului este obligatoriu")]
            public DateTime Date { get; set; }
            public string Content { get; set; }
            public virtual ICollection<Comment> Comments { get; set; }

    }
}
