using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{
    [Table("EX_Exams")]
    public class EX_Exam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamID { get; set; }

 /*       [Required]
        [StringLength(255)]
        public string Code { get; set; }*/

        [Required]
        [StringLength(255)]
        public string ExamName { get; set; }

        [Required]
        public DateTime ExamDate { get; set; }

        public int SubjectID { get; set; }
        public int TotalMins { get; set; }

        public DateTime? TimeEnd { get; set; }
        public int Status { get; set; }
        [ForeignKey("SubjectID")]
        public virtual EX_Subject Subject { get; set; }

        public virtual ICollection<EX_ExamQuestion> ExamQuestions { get; set; }
    }
}
