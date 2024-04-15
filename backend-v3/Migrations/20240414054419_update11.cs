using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_v3.Migrations
{
    /// <inheritdoc />
    public partial class update11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "tbl_YKienGopY",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "tbl_YKienGopY",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "tbl_YKienGopY",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "tbl_YKienGopY",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "tbl_YKienGopY");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "tbl_YKienGopY");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "tbl_YKienGopY");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "tbl_YKienGopY");
        }
    }
}
