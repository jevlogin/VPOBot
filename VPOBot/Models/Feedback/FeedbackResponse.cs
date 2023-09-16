using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class FeedbackResponse
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponseId { get; set; }

        public DateTime ResponseDateTime { get; set; }

        public long? UserId { get; set; }

        public ResponseData ResponseData { get; set; }

        [ForeignKey("UserId")]
        public UserVPO User { get; set; }
    }
}
