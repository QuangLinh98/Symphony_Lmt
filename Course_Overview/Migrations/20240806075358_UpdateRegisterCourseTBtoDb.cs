using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Overview.Migrations
{
	/// <inheritdoc />
	public partial class UpdateRegisterCourseTBtoDb : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
		   name: "ID",
		   table: "RegistrationCourses",
		   type: "int",
		   nullable: false,
		   defaultValue: 0);
		}
	}
}
