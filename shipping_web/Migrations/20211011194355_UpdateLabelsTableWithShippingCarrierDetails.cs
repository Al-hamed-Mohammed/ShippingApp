using Microsoft.EntityFrameworkCore.Migrations;

namespace Shipping_Label_App.Migrations
{
    public partial class UpdateLabelsTableWithShippingCarrierDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarrierName",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "ShipingRate",
                table: "Labels",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarrierName",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "ShipingRate",
                table: "Labels");
        }
    }
}
