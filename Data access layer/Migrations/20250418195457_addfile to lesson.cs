using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_access_layer.Migrations
{
    /// <inheritdoc />
    public partial class addfiletolesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskFileName",
                table: "Lessons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskFileName",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Courses");
        }
    }
}
