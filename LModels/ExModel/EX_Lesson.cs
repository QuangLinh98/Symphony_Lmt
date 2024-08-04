using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{
    [Table("EX_Lessons")]
    public class EX_Lesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LessonID { get; set; }

        [Required]
        [StringLength(255)]
        public string LessonName { get; set; }

        public int SubjectID { get; set; }
        [StringLength(255)]
        public string? LessonNumber { get; set; }

        [ForeignKey("SubjectID")]
        public virtual EX_Subject Subject { get; set; }

        public virtual ICollection<EX_Question> Questions { get; set; }
    }
}
