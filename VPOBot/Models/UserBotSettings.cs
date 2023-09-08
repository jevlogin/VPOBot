using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WORLDGAMEDEVELOPMENT
{
    internal sealed class UserBotSettings : EmptyBotSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }

        public TimeSpan? MorningTime { get; set; }
        public TimeSpan? EveningTime { get; set; }


        [ForeignKey("UserId")]
        public UserVPO User { get; set; }


        public override string ToString()
        {
            return $"<b>Самое ранне сообщение с</b>: {MorningTime} часов утра.\n\n<b>Самое позднее до</b>: {EveningTime} часов";
        }
    }
}
