using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_social_platform.Models
{
    public class UserGroup
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? GroupId { get; set; }
        public string? UserId { get; set; } 
        public virtual Group? Group { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public DateTime GroupDate { get; set; }
    }
}
