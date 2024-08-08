using LModels.ExModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LModels
{
	public class Class : IValidatableObject
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ClassID { get; set; }

		[Required(ErrorMessage = "Class name is required")]
		public string ClassName { get; set; }
		public int? TeacherID { get; set; }

		[Required]
		[Column(TypeName = "decimal(10,2)")]
		public decimal Fee { get; set; }

		[Required]
		[RegularExpression("^(Basic|Advanced)$", ErrorMessage = "Invalid Segregated Class")]
		public string ClassType { get; set; }  //Phân loai lớp

		[Required]
		public DateOnly StartDate { get; set; }

		[Required]
		public DateOnly EndDate { get; set; }

		[Required]
		[DataType(DataType.MultilineText)]
		public string Description { get; set; }

		public Teacher? Teacher { get; set; }
		public ICollection<Schedule>? Schedules { get; set; }
		public ICollection<ExamResult>? ExamResults { get; set; }
		public ICollection<ClassStudent>? ClassStudents { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
            // Kiểm tra StartDate không phải là ngày đã qua
            if (StartDate < DateOnly.FromDateTime(DateTime.Today))
            {
                yield return new ValidationResult(
                    "StartDate must be today or a future date",
                    new[] { nameof(StartDate) }
                );
            }

            // Kiểm tra EndDate phải lớn hơn StartDate
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "EndDate must be greater than StartDate",
                    new[] { nameof(EndDate) }
                );
            }
        }
	}
}
