

namespace LModels
{
	public class EntranceExam
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ExamID { get; set; }
		public DateTime ExamDate { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal Fee { get; set; }       // Phí kỳ thi tuyển sinh
		public DateTime StartTime { get; set; }       // thời gian bắt đầu thi
		public DateTime EndTime { get; set; }       // Phí kỳ thi tuyển sinh

		[Required]
		[RegularExpression("^(Complete|InComplete)$", ErrorMessage = "Invalid Entrance Exam Status")]
		public string Status { get; set; }       // Trạng thái (Hoàn thành , chưa hoàn thành<đang diễn ra>) 

		//public ICollection<Student>? Students { get; set; }
		public ICollection<ExamResult>? ExamResults { get; set; }
	}
}
