using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniTwit.Entities
{
    public class Follows
    {
        public int FollowerId { get; set; }
        public int FolloweeId { get; set; }
        public User Follower { get; set; }
        public User Followee { get; set; }
    }
}