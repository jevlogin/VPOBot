using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WORLDGAMEDEVELOPMENT
{
    public class UserVPO
    {
        [Key]
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }


        [InverseProperty("User")]
        public ICollection<FeedbackResponse>? FeedbackResponses { get; set; }
    }
}
