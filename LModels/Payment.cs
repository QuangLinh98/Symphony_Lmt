

namespace LModels
{
	public class Payment
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PaymentID { get; set; }
		public int StudentID { get; set; }

		[Required]
		[Column(TypeName ="Decimal(10,2)")]
		public decimal Amount { get; set; }
		public string PaymentMethod { get; set; }
		public DateTime PaymentDate { get; set; }
		public string ReceiptNumber { get; set; }

		//Tạo Contructor để thưc hiện Generate IdentityCard 
		public Payment()
		{
			ReceiptNumber = GenerateReceiptNumber();
		}

		// Sử dụng biểu thức điều kiện đê thực hiện Generate
		private string GenerateReceiptNumber()
		{
			DateTime now = DateTime.Now;
			return $"P{now:yyyyMMddHHmmss}";
		}

		//public Student? Student { get; set; }
	}
}

