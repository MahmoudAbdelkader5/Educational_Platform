using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_access_layer.Migrations
{
    /// <inheritdoc />
    public partial class upadateexam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxGrade",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "dateTime",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "startDate",
                table: "Exams");

            migrationBuilder.AddColumn<int>(
                name: "duration",
                table: "Exams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "Exams");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxGrade",
                table: "Exams",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateTime",
                table: "Exams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "Exams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
