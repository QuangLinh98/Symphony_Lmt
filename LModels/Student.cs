

using LModels.ExModel;
using Microsoft.AspNetCore.Http;

namespace LModels
{
    public class Student 
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
        [Phone]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Phone number must start with 0 and be 10 to 11 digits long.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }


        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string IdentityCard { get; set; }

        //Tạo Contructor để thưc hiện Generate IdentityCard 
        public Student()
        {
            IdentityCard = GenerateIdentityCard();
        }

        // Sử dụng biêyr thức điều kiện đê thực hiện Generate
        private string GenerateIdentityCard()
        {
            DateTime now = DateTime.Now;
            return $"SV{now:yyyyMMddHHmmss}";
        }

		public int FailedAttempts { get; set; }   // Thuộc tính để lưu trữ số lần đăng nhập thất bại
		public DateTime? LockoutEnd { get; set; } // Thời gian khóa tài khoản kết thúc

		public bool EmailConfirmed { get; set; }     //cho biết địa chỉ email của người dùng đã được xác nhận hay chưa.
		public string? EmailConfirmationToken { get; set; }   //lưu trữ mã thông báo được sử dụng để xác nhận địa chỉ email của người dùng

		public string? ResetPasswordToken { get; set; }      // MÃ token gửi cho User để Reset Password
		public DateTime? ResetPasswordTokenExpiration { get; set; }
		public bool IsNewUser { get; set; }                 // Chức năng: Để set điều kiện user mới đăng ký phải xác thực email mới cho phép login và user cần resetPassword có thể login sau khi reset mà không cần xác thực   

		public ICollection<LabSession>? LabSessions { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<ExamResult>? ExamResults { get; set; }
        public ICollection<SubjectScores>? SubjectScores { get; set; }
        public ICollection<FeedBack>? FeedBacks { get; set; }
        public virtual ICollection<ClassStudent>? ClassStudents { get; set; }
        
    }
}
