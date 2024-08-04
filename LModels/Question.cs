

namespace LModels
{
	public class Question
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int QuestionID { get; set; }

		[Required]
		public string QuestionText { get; set; }    //Nội dung câu hỏi
		public int CorrectAnswerID { get; set; }   //Liên kết với bảng Answer

	}
}
