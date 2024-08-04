using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{
    [Table("EX_Subjects")]
    public class EX_Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectID { get; set; }

        [Required]
        [StringLength(255)]
        public string SubjectName { get; set; }

        public virtual ICollection<EX_Lesson> Lessons { get; set; }
        public virtual ICollection<EX_Exam> Exams { get; set; }
    }
}
