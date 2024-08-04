

namespace LModels.Client
{
	public class Services
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceID { get; set; }

		[Required(ErrorMessage = "Icon is required")]
		public string Icon { get; set; }

		[Required(ErrorMessage = "Title is required")]
		public string Title { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }
    }
}
