using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeFirstFromDbDemo.Migrations
{
    public partial class RenameTitleToNameInCoursesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Courses",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Courses",
                newName: "Title");
        }
    }
}
