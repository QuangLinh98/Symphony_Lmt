using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{
    [Table("EX_ExamQuestions")]
    public class EX_ExamQuestion
    {
        public int ExamID { get; set; }

        [ForeignKey("ExamID")]
        public virtual EX_Exam Exam { get; set; }

        public int QuestionID { get; set; }

        [ForeignKey("QuestionID")]
        public virtual EX_Question Question { get; set; }
    }
}
