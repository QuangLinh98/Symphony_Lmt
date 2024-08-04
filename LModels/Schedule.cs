

namespace LModels
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleID { get; set; }

        [Required]
        public int CourseID { get; set; }

        [Required]
        public int ClassID { get; set; }

        [Required(ErrorMessage = "StartDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "StartDate must be a valid date.")]
        public DateTime StartDate { get; set; }     //Ngày bắt đầu môn học 

        [Required(ErrorMessage = "EndDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "EndDate must be a valid date.")]
        public DateTime EndDate { get; set; }       //Ngày kết thúc môn học

        [Required(ErrorMessage = "StartTime is required.")]
        [DataType(DataType.Time, ErrorMessage = "StartTime must be a valid time.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "EndTime is required.")]
        [DataType(DataType.Time, ErrorMessage = "EndTime must be a valid time.")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "RecurringDays is required.")]
        [RegularExpression(@"^(Mon|Tue|Wed|Thu|Fri|Sat|Sun)(,(Mon|Tue|Wed|Thu|Fri|Sat|Sun))*$", ErrorMessage = "RecurringDays must be a comma-separated list of valid days (e.g., Mon,Wed,Fri).")]
        public string RecurringDays { get; set; }     //Ví dụ: "Mon,Wed,Fri"

        [Required(ErrorMessage = "Room is required.")]
        [StringLength(50, ErrorMessage = "Room cannot be longer than 50 characters.")]
        public string Room { get; set; }
        public Course? Course {  get; set; } 
        public Class? Class { get; set; }
    }
}
