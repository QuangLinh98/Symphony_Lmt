using LModels;
using LModels.Client;
using LModels.ExModel;
using Microsoft.EntityFrameworkCore;

namespace Course_Overview.Data
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Định nghĩa khóa chính cho UserRole
			modelBuilder.Entity<UserRole>()
				.HasKey(ur => new { ur.UserId, ur.RoleId });

			modelBuilder.Entity<UserRole>()
				.HasOne(ur => ur.User)
				.WithMany(u => u.UserRoles)
				.HasForeignKey(ur => ur.UserId);

			modelBuilder.Entity<UserRole>()
				.HasOne(ur => ur.Role)
				.WithMany(r => r.UserRoles)
				.HasForeignKey(ur => ur.RoleId);

			// Định nghĩa khóa chính cho RolePermission
			modelBuilder.Entity<RolePermission>()
				.HasKey(rp => new { rp.RoleId, rp.PermissionId });

			modelBuilder.Entity<RolePermission>()
				.HasOne(rp => rp.Role)
				.WithMany(r => r.RolePermissions)
				.HasForeignKey(rp => rp.RoleId);

			modelBuilder.Entity<RolePermission>()
				.HasOne(rp => rp.Permission)
				.WithMany(p => p.RolePermissions)
				.HasForeignKey(rp => rp.PermissionId);

			// Cấu hình Bảng Student cho 2 trường là duy nhất 
			modelBuilder.Entity<Student>()
				.HasIndex(s => s.Email)
				.IsUnique();

			modelBuilder.Entity<Student>()
				.HasIndex(s => s.Phone)
				.IsUnique();

			// Cấu hình Bảng Teacher cho 2 trường là duy nhất 
			modelBuilder.Entity<Teacher>()
				.HasIndex(t => t.Email)
				.IsUnique();

			modelBuilder.Entity<Teacher>()
				.HasIndex(t => t.Phone)
				.IsUnique();

			// Cấu hình Bảng ContacInfo cho 2 trường là duy nhất 
			modelBuilder.Entity<Contact>()
				.HasIndex(ci => ci.Phone)
				.IsUnique();

			modelBuilder.Entity<Contact>()
				.HasIndex(ci => ci.Phone1)
				.IsUnique();

			modelBuilder.Entity<Contact>()
				.HasIndex(ci => ci.Phone2)
				.IsUnique();

			// Cấu hình Bảng ContacInfo cho 2 trường là duy nhất

			modelBuilder.Entity<User>()
				.HasIndex(t => t.Email)
				.IsUnique();

			modelBuilder.Entity<User>()
				.HasIndex(t => t.Phone)
				.IsUnique();

            // Cấu hình mối quan hệ giữa Course và Topic
            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Course)
                .WithMany(c => c.Topics)
                .HasForeignKey(t => t.CourseID);

            //Cấu hình mối quan hệ giữa Schedule với Course
            modelBuilder.Entity<Schedule>()
                .HasOne(t => t.Course)
                .WithMany(c => c.Schedules)
                .HasForeignKey(t => t.CourseID);

            //Cấu hình mối quan hệ giữa Schedule với Class
            modelBuilder.Entity<Schedule>()
                .HasOne(t => t.Class)
                .WithMany(cl => cl.Schedules)
                .HasForeignKey(t => t.ClassID);

            //Cấu hình mối quan hệ giữa bảng RegistrationCourse vơi Course
            modelBuilder.Entity<RegistrationCourse>()
                .HasOne(rc  => rc.Course)
                .WithMany(c => c.RegistrationCourses)
                .HasForeignKey(rc => rc.CourseID);

            //Cấu hình mối quan hệ giữa bảng RegistrationCourse vơi User
            modelBuilder.Entity<RegistrationCourse>()
                .HasOne(rc => rc.User)
                .WithMany(u => u.RegistrationCourses)
                .HasForeignKey(rc => rc.ID);

            // EX_Subject - EX_Lessons
            modelBuilder.Entity<EX_Subject>()
                .HasMany(s => s.Lessons)
                .WithOne(l => l.Subject)
                .HasForeignKey(l => l.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            // EX_Lesson - EX_Questions
            modelBuilder.Entity<EX_Lesson>()
                .HasMany(l => l.Questions)
                .WithOne(q => q.Lesson)
                .HasForeignKey(q => q.LessonID)
                .OnDelete(DeleteBehavior.Restrict);

            // EX_Question - EX_Answers
            modelBuilder.Entity<EX_Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionID)
                .OnDelete(DeleteBehavior.Cascade);

            // EX_Subject - EX_Exams
            modelBuilder.Entity<EX_Subject>()
                .HasMany(s => s.Exams)
                .WithOne(e => e.Subject)
                .HasForeignKey(e => e.SubjectID)
                .OnDelete(DeleteBehavior.Restrict);

            // EX_Exam - EX_ExamQuestions
            modelBuilder.Entity<EX_ExamQuestion>()
                .HasKey(eq => new { eq.ExamID, eq.QuestionID });

            modelBuilder.Entity<EX_ExamQuestion>()
                .HasOne(eq => eq.Exam)
                .WithMany(e => e.ExamQuestions)
                .HasForeignKey(eq => eq.ExamID);

            modelBuilder.Entity<EX_ExamQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamQuestions)
                .HasForeignKey(eq => eq.QuestionID);

            //

            modelBuilder.Entity<ClassExam>()
           .HasKey(ce => new { ce.ClassID, ce.ExamID });

            modelBuilder.Entity<ClassExam>()
                .HasOne(ce => ce.Class)
                .WithMany() // Adjust based on your relationship
                .HasForeignKey(ce => ce.ClassID);

            modelBuilder.Entity<ClassExam>()
                .HasOne(ce => ce.Exam)
                .WithMany() // Adjust based on your relationship
                .HasForeignKey(ce => ce.ExamID);

            //
            modelBuilder.Entity<ClassStudent>(entity =>
            {
                entity.HasKey(e => new { e.ClassID, e.StudentID });

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.ClassStudents)
                    .HasForeignKey(d => d.ClassID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.ClassStudents)
                    .HasForeignKey(d => d.StudentID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EX_StudentExamResult>()
                .HasKey(e => new { e.StudentID, e.ExamID, e.QuestionID });

        }

        public DbSet<Course> Courses { get; set; }                  //Bảng cho tiết khóa học (Java)
		public DbSet<Topic> Topics { get; set; }                    //Bảng chủ đề khóa học (Java introdule)
		public DbSet<RegistrationCourse> RegistrationCourses { get; set; }                    //Bảng đăng ký khóa học
        public DbSet<EntranceExam> EntranceExams { get; set; }      // Bảng kỳ thi tuyển sinh
        public DbSet<ExamResult> ExamResults { get; set; }          //Bảng lưu kết quả thi, phân lớp của học viên 
        public DbSet<FAQ> FAQs { get; set; }                        //Bảng câu hỏi thường gặp 
        public DbSet<Class> Classes { get; set; }                   //Bảng lớp học
        public DbSet<LabSession> LabSessions { get; set; }          //Bảng phòng Lab
        public DbSet<Student> Students { get; set; }                //Bảng học viên
        public DbSet<Teacher> Teachers { get; set; }                //Bảng giáo viên
        public DbSet<Payment> Payments { get; set; }                //Bảng thanh toán 
        public DbSet<Question> Questions { get; set; }              //Bảng lưu câu hỏi cho sinh viên làm bài thi

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<SubjectScores> SubjectScores { get; set; }
        public DbSet<Slider> Sliders { get; set; }                    //Bảng Slider
        public DbSet<Services> Services { get; set; }                    //Bảng Service

        //Viễn Entity
        public DbSet<EX_Subject> EX_Subjects { get; set; }
        public DbSet<EX_Lesson> EX_Lessons { get; set; }
        public DbSet<EX_Question> EX_Questions { get; set; }
        public DbSet<EX_Answer> EX_Answers { get; set; }
        public DbSet<EX_Exam> EX_Exams { get; set; }
        public DbSet<EX_ExamQuestion> EX_ExamQuestions { get; set; }
        public DbSet<ClassExam> ClassExams { get; set; }
		public DbSet<ClassStudent> ClassStudents { get; set; }
		public DbSet<EX_StudentExamResult> EX_StudentExamResults { get; set; }
		public DbSet<AQ> AQs { get; set; }
    }
}
