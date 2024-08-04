

namespace LModels
{
	public class FeedBack
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int FeedBackId { get; set; }
		public int StudentId { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Content { get; set; }

		public DateTime Date_Posted { get; set; }

		public Student? Student { get; set; }
	}
}
