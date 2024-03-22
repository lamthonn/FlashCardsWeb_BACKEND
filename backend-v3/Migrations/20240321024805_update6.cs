using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend_v3.Migrations
{
    /// <inheritdoc />
    public partial class update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoTheHoc",
                table: "tbl_HocPhan",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoTheHoc",
                table: "tbl_HocPhan");
        }
    }
}
