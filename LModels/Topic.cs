
using Microsoft.AspNetCore.Http;

namespace LModels
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopicID { get; set; }
        public int CourseID { get; set; }

        [Required(ErrorMessage = "Course Name is required")]
        public string TopicName { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public Course? Course { get; set; }
        public ICollection<SubjectScores>? SubjectScores { get; set; }
        
    }
}
