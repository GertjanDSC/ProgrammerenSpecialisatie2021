using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeFirstFromDbDemo.Migrations
{
    public partial class DeleteDatePublishedColumnFromCoursesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatePublished",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DatePublished",
                table: "Courses",
                type: "datetime2",
                nullable: true);
        }
    }
}
