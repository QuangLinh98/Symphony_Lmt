

namespace LModels
{
	public class FAQ
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int FAQID { get; set; }

		[Required(ErrorMessage = "Question is required")]
		[StringLength(255, ErrorMessage = "Question cannot exceed 255 characters")]
		public string Question { get; set; }

		[Required(ErrorMessage = "Answer is required")]
		[StringLength(1000, ErrorMessage = "Answer cannot exceed 1000 characters")]
		public string Answer { get; set; }
	}
}
