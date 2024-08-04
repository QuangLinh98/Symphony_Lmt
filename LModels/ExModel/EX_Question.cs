using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{
    [Table("EX_Questions")]
    public class EX_Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionID { get; set; }

        [Required]
        public string QuestionText { get; set; }

        public int LessonID { get; set; }

        [ForeignKey("LessonID")]
        public virtual EX_Lesson Lesson { get; set; }

        public virtual ICollection<EX_Answer> Answers { get; set; }
        public virtual ICollection<EX_ExamQuestion> ExamQuestions { get; set; }
    }
}
