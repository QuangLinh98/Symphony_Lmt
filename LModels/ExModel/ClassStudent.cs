using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LModels.ExModel
{
	[Table("ClassStudents")]
	public class ClassStudent
	{
		[Key, Column(Order = 0)]
		public int ClassID { get; set; }

		[Key, Column(Order = 1)]
		public int StudentID { get; set; }
		public bool? Status { get; set; }
		// Navigation properties
		[ForeignKey("ClassID")]
		public virtual Class Class { get; set; }

		[ForeignKey("StudentID")]
		public virtual Student Student { get; set; }
	}
}
