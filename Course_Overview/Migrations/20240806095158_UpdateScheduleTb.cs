using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Overview.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScheduleTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentID",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_StudentID",
                table: "Schedules",
                column: "StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Students_StudentID",
                table: "Schedules",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "StudentID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Students_StudentID",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_StudentID",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "Schedules");
        }
    }
}
