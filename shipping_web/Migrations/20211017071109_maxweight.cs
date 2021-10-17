using Microsoft.EntityFrameworkCore.Migrations;

namespace Shipping_Label_App.Migrations
{
    public partial class maxweight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Maxweight",
                table: "providers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Maxweight",
                table: "providers");
        }
    }
}
