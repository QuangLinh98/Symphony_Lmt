using LModels;

namespace Course_Overview.ViewModel
{
    public class EditUserViewModel
    {
        public string? Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
