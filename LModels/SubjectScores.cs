

namespace LModels
{

    //Bảng điểm môn học 
    public class SubjectScores
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectScoresID { get; set; }
        public int StudentID { get; set; }
        public int TopicID { get; set; }

        [Column(TypeName ="Decimal(10,2)")]
        public decimal Mark { get; set; }

       // public Student? Student { get; set; }
        public Topic? Topic { get; set; }
    }
}
