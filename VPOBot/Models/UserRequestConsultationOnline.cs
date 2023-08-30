using System.ComponentModel.DataAnnotations;


namespace WORLDGAMEDEVELOPMENT
{
    internal class UserRequestConsultationOnline
    {
        [Key]
        public long UserId { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }

        [Required]
        public string Phone { get; set; }
        public string? Email { get; set; }
    }
}
