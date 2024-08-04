using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{


    [Table("EX_StudentExamResults")]
    public class EX_StudentExamResult
    {
        [Key, Column(Order = 0)]
        public int StudentID { get; set; }

        [Key, Column(Order = 1)]
        public int ExamID { get; set; }

        [Key, Column(Order = 2)]
        public int QuestionID { get; set; }

        public int StudentAnswers { get; set; } 

        [Required]
        public bool IsCorrect { get; set; }
        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }

        [ForeignKey("ExamID")]
        public virtual EX_Exam Exam { get; set; }
    }

}
