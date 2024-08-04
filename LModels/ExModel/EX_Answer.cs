using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{
    [Table("EX_Answers")]
    public class EX_Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerID { get; set; }

        [Required]
        public string AnswerText { get; set; }

        [Required]
        public bool IsCorrect { get; set; }

        public int QuestionID { get; set; }

        [ForeignKey("QuestionID")]
        public virtual EX_Question Question { get; set; }
    }
}
