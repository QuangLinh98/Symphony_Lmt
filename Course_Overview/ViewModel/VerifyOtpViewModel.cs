using System.ComponentModel.DataAnnotations;

namespace Course_Overview.ViewModel
{
    public class VerifyOtpViewModel
    {
        [Required(ErrorMessage = "OTP is required.")]
        public string Otp { get; set; }
    }
}
