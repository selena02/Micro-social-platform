using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Micro_social_platform.Models
{
    public class Profile
    {
        [Key]

        public int ProfileId { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string ProfilePicture { get; set; } 
        public string UserName  { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime JoinDate { get; set; }  

    }
}
