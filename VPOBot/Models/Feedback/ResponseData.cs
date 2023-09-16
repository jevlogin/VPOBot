using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WORLDGAMEDEVELOPMENT
{
    public class ResponseData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponseDataId { get; set; }
        public List<QuestionAnswerPair>? Responses { get; set; }

        public int ResponseId { get; set; }

        [ForeignKey("ResponseId")]
        public FeedbackResponse FeedbackResponse { get; set; }
    }
}
