using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shipping_Label_App.Migrations
{
    public partial class AddedTrackingNoFieldsto_Label_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Labels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Datecreated",
                table: "Labels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Labels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TrackingNo",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "Datecreated",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "TrackingNo",
                table: "Labels");
        }
    }
}
