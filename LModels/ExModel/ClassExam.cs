using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{
    [Table("ClassExams")]
    public class ClassExam
    {
        [Key, Column(Order = 0)]
        public int ClassID { get; set; }

        [Key, Column(Order = 1)]
        public int ExamID { get; set; }

        // Navigation properties
        [ForeignKey("ClassID")]
        public virtual Class Class { get; set; }

        [ForeignKey("ExamID")]
        public virtual EX_Exam Exam { get; set; }
    }
}
