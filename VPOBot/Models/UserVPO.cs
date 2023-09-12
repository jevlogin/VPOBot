using System.ComponentModel.DataAnnotations;


namespace WORLDGAMEDEVELOPMENT
{
    public class UserVPO
    {
        [Key]
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
    }
}
