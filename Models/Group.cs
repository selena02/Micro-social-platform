using System.ComponentModel.DataAnnotations;


namespace Micro_social_platform.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Group must have a title")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Group must have a description")]
        public string Description { get; set; }
        public virtual ICollection<GroupMessage>? GroupMessages { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string? UserId { get; set; }
        public virtual ICollection<UserGroup>? UserGroups 
        { get; set; }


    }
}
