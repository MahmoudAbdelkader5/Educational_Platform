using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_access_layer.Migrations
{
    /// <inheritdoc />
    public partial class mnmnmnmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Subjects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    q1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    q2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    q3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    q4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionID);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FatherPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GradeLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxGrade = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VideoURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TaskFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportingFiles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    num_of_views = table.Column<int>(type: "int", nullable: false),
                    Create_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Lessons_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Revisions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Video = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Files = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revisions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Revisions_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StudentId = table.Column<int>(type: "int", nullable: true),
                    InstructorId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamQuestions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mark = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExamID = table.Column<int>(type: "int", nullable: false),
                    QuestionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamQuestions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExamQuestions_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamQuestions_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentExams",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ExamDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentExams", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StudentExams_Exams_ExamID",
                        column: x => x.ExamID,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentExams_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxGrade = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    LessonID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Assignments_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Lessons_LessonID",
                        column: x => x.LessonID,
                        principalTable: "Lessons",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    InstructorID = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reply = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_Instructors_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "Instructors",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Comments_Lessons_LessonID",
                        column: x => x.LessonID,
                        principalTable: "Lessons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    examQuestionID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionsQuestionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Answers_ExamQuestions_examQuestionID",
                        column: x => x.examQuestionID,
                        principalTable: "ExamQuestions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionsQuestionID",
                        column: x => x.QuestionsQuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID");
                    table.ForeignKey(
                        name: "FK_Answers_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentQuestions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentID = table.Column<int>(type: "int", nullable: false),
                    QuestionID = table.Column<int>(type: "int", nullable: false),
                    mark = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentQuestions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AssignmentQuestions_Assignments_AssignmentID",
                        column: x => x.AssignmentID,
                        principalTable: "Assignments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentQuestions_Questions_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "Questions",
                        principalColumn: "QuestionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAssignments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    SubmittedFiles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAssignments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_StudentAssignments_Assignments_AssignmentID",
                        column: x => x.AssignmentID,
                        principalTable: "Assignments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAssignments_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentAnswers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    assignment_question_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentAnswers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AssignmentAnswers_AssignmentQuestions_assignment_question_id",
                        column: x => x.assignment_question_id,
                        principalTable: "AssignmentQuestions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentAnswers_Students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_examQuestionID",
                table: "Answers",
                column: "examQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionsQuestionID",
                table: "Answers",
                column: "QuestionsQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_StudentID",
                table: "Answers",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InstructorId",
                table: "AspNetUsers",
                column: "InstructorId",
                unique: true,
                filter: "[InstructorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StudentId",
                table: "AspNetUsers",
                column: "StudentId",
                unique: true,
                filter: "[StudentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAnswers_assignment_question_id",
                table: "AssignmentAnswers",
                column: "assignment_question_id");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAnswers_StudentID",
                table: "AssignmentAnswers",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentQuestions_AssignmentID",
                table: "AssignmentQuestions",
                column: "AssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentQuestions_QuestionID",
                table: "AssignmentQuestions",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CourseID",
                table: "Assignments",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_LessonID",
                table: "Assignments",
                column: "LessonID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_InstructorID",
                table: "Comments",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LessonID",
                table: "Comments",
                column: "LessonID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_StudentID",
                table: "Comments",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseID",
                table: "Enrollments",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentID",
                table: "Enrollments",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamID",
                table: "ExamQuestions",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_QuestionID",
                table: "ExamQuestions",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_CourseId",
                table: "Exams",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseID",
                table: "Lessons",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Revisions_CourseID",
                table: "Revisions",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignments_AssignmentID",
                table: "StudentAssignments",
                column: "AssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAssignments_StudentID",
                table: "StudentAssignments",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExams_ExamID",
                table: "StudentExams",
                column: "ExamID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentExams_StudentID",
                table: "StudentExams",
                column: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AssignmentAnswers");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Revisions");

            migrationBuilder.DropTable(
                name: "StudentAssignments");

            migrationBuilder.DropTable(
                name: "StudentExams");

            migrationBuilder.DropTable(
                name: "ExamQuestions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AssignmentQuestions");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
