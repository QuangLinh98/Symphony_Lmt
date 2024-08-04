using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Overview.Migrations
{
    /// <inheritdoc />
    public partial class TableRegisCourseDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
	  name: "RegistrationCourses",
	  columns: table => new
	  {
		  RegistrationCourseID = table.Column<int>(type: "int", nullable: false)
			  .Annotation("SqlServer:Identity", "1, 1"),
		  Status = table.Column<string>(type: "nvarchar(max)", nullable: true), // Nullable nếu không bắt buộc
		  CourseID = table.Column<int>(type: "int", nullable: false),
		  ID = table.Column<int>(type: "int", nullable: false),
		  RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
	  },
	  constraints: table =>
	  {
		  table.PrimaryKey("PK_RegistrationCourses", x => x.RegistrationCourseID);
		  table.ForeignKey(
			  name: "FK_RegistrationCourses_Courses_CourseID",
			  column: x => x.CourseID,
			  principalTable: "Courses", // Tên bảng Courses
			  principalColumn: "CourseID",
			  onDelete: ReferentialAction.Cascade);
		  table.ForeignKey(
			  name: "FK_RegistrationCourses_Users_ID",
			  column: x => x.ID,
			  principalTable: "Users", // Tên bảng Users
			  principalColumn: "ID",
			  onDelete: ReferentialAction.Cascade);
	  });

			migrationBuilder.CreateIndex(
				name: "IX_RegistrationCourses_CourseID",
				table: "RegistrationCourses",
				column: "CourseID");

			migrationBuilder.CreateIndex(
				name: "IX_RegistrationCourses_ID",
				table: "RegistrationCourses",
				column: "ID");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
