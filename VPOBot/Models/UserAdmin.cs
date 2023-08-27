using System.ComponentModel.DataAnnotations;


namespace WORLDGAMEDEVELOPMENT
{
    public class UserAdmin
    {
        [Key, Required(ErrorMessage = "Please input UserId")] 
        public long UserId { get; set; }
        [Required(ErrorMessage = "Please enter the name Administrator")]
        public string UserName { get; set; }
    }
}
