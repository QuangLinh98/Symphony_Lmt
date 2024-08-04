

namespace LModels
{
	public class ExamResult
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ResultID { get; set; }
		public int StudentID { get; set; }
		public int ExamID { get; set; }
        public int ClassID { get; set; }       //Phân loại lớp

        [Required]
		[Range(0, 10, ErrorMessage = "Mark must be between from 0 to 10")]
		public double Marks { get; set; }               //Điểm số

		[Required]
		[RegularExpression("^(Paid|Unpaid)$", ErrorMessage = "Invalid Payment Status")]
		public string PaymentStatus { get; set; }     // Trạng thái thanh toán học phí của học viên (ví dụ: "Paid", "Unpaid").

        //public Student? Student { get; set; }
		public EntranceExam? EntranceExam { get; set; }
		public Class? Class { get; set; }
	}
}
