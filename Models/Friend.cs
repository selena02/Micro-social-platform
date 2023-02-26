using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Micro_social_platform.Models
{

    public class Friend
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public string? User_sender_id { get; set; }

        public string? User_receiver_id { get; set; }

        public bool? Has_accepted { get; set; }

        public string? User_sender_name { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }

        public DateTime FriendDate { get; set; }
    }
}