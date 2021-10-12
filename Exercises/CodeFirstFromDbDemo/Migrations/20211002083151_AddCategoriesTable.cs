using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeFirstFromDbDemo.Migrations
{
    public partial class AddCategoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('Web Development')");
            migrationBuilder.Sql("INSERT INTO Categories (Name) VALUES ('Programming Languages')");
            */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
