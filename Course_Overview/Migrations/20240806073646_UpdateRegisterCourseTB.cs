using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Overview.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRegisterCourseTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			// Cập nhật các giá trị ID trong RegistrationCourses thành StudentID hợp lệ trước khi đổi tên
			migrationBuilder.Sql(@"
                UPDATE rc
                SET ID = s.StudentID
                FROM RegistrationCourses rc
                INNER JOIN Students s ON rc.ID = s.StudentID
                WHERE rc.ID IS NOT NULL;
            ");

			// Đổi tên cột ID thành StudentID trong bảng RegistrationCourses
			migrationBuilder.RenameColumn(
				name: "ID",
				table: "RegistrationCourses",
				newName: "StudentID");

			// Đổi tên chỉ mục liên quan từ ID thành StudentID
			migrationBuilder.RenameIndex(
				name: "IX_RegistrationCourses_ID",
				table: "RegistrationCourses",
				newName: "IX_RegistrationCourses_StudentID");

			// Đảm bảo tất cả giá trị StudentID hợp lệ trong RegistrationCourses
			migrationBuilder.Sql(@"
                DELETE FROM RegistrationCourses
                WHERE StudentID NOT IN (SELECT StudentID FROM Students);
            ");

			// Thêm khóa ngoại liên kết bảng RegistrationCourses với Students qua cột StudentID
			migrationBuilder.AddForeignKey(
				name: "FK_RegistrationCourses_Students_StudentID",
				table: "RegistrationCourses",
				column: "StudentID",
				principalTable: "Students",
				principalColumn: "StudentID",
				onDelete: ReferentialAction.Cascade);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropForeignKey(
			   name: "FK_RegistrationCourses_Students_StudentID",
			   table: "RegistrationCourses");

			migrationBuilder.RenameColumn(
				name: "StudentID",
				table: "RegistrationCourses",
				newName: "ID");

			migrationBuilder.RenameIndex(
				name: "IX_RegistrationCourses_StudentID",
				table: "RegistrationCourses",
				newName: "IX_RegistrationCourses_ID");

			migrationBuilder.AddForeignKey(
				name: "FK_RegistrationCourses_Users_ID",
				table: "RegistrationCourses",
				column: "ID",
				principalTable: "Users",
				principalColumn: "ID",
				onDelete: ReferentialAction.Cascade);
		}
    }
}
