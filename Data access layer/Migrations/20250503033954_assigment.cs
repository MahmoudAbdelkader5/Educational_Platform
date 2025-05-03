using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_access_layer.Migrations
{
    /// <inheritdoc />
    public partial class assigment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmittedFiles",
                table: "StudentAssignments");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Instructions",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Assignments");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<int>(
                name: "duration",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "Assignments");

            migrationBuilder.AddColumn<string>(
                name: "SubmittedFiles",
                table: "StudentAssignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Assignments",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Assignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instructions",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
