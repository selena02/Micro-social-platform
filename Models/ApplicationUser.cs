using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_social_platform.Models
{
    public class ApplicationUser:IdentityUser
    {
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Article>? Articles { get; set; }
        public virtual ICollection<Profile>? Profiles { get; set; }
        public virtual ICollection<Group>? Groups { get; set; }
        public virtual ICollection<GroupMessage>? GroupMessages { get; set; }

        public virtual ICollection<Friend>? Friends { get; set; } 

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; } 

    }
}
