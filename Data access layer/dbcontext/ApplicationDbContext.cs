// ApplicationDbContext.cs
using Data_access_layer.model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    // DbSets for all your entities
    public DbSet<Message> Message { get; set; }
    public DbSet<student_answers> Answers { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<assignment_question> AssignmentQuestions { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<student_Course> Enrollments { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<examQuestion> ExamQuestions { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Questions> Questions { get; set; }
    public DbSet<Revision> Revisions { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Student_Assignment> StudentAssignments { get; set; }
    public DbSet<Student_Exam> StudentExams { get; set; }
    public DbSet<assignment_Answer> AssignmentAnswers { get; set; }
    public DbSet<EmailConfirmationToken> EmailConfirmationTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Instructor>()
            .HasOne(i => i.User)
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Student)
            .WithOne(s => s.User)
            .HasForeignKey<ApplicationUser>(a => a.StudentId)
            .OnDelete(DeleteBehavior.Restrict); // Optional, but safer

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Instructor)
            .WithOne(i => i.User)
            .HasForeignKey<ApplicationUser>(a => a.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<EmailConfirmationToken>()
    .HasOne(t => t.User)
    .WithMany()
    .HasForeignKey(t => t.UserId)
    .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PasswordResetToken>()
    .HasOne(t => t.User)
    .WithMany()
    .HasForeignKey(t => t.UserId)
    .OnDelete(DeleteBehavior.Cascade);
    }

}