using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_access_layer.Migrations
{
    /// <inheritdoc />
    public partial class nullablefeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "StudentAssignments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "QuestionsQuestionID",
                table: "AssignmentAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAnswers_QuestionsQuestionID",
                table: "AssignmentAnswers",
                column: "QuestionsQuestionID");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentAnswers_Questions_QuestionsQuestionID",
                table: "AssignmentAnswers",
                column: "QuestionsQuestionID",
                principalTable: "Questions",
                principalColumn: "QuestionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentAnswers_Questions_QuestionsQuestionID",
                table: "AssignmentAnswers");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentAnswers_QuestionsQuestionID",
                table: "AssignmentAnswers");

            migrationBuilder.DropColumn(
                name: "QuestionsQuestionID",
                table: "AssignmentAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "StudentAssignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
