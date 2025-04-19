using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.model
{
    // Student.cs
    public class Student
    {

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [StringLength(20)]
        public string FatherPhone { get; set; }


        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string GradeLevel { get; set; }

        [StringLength(255)]
        public string ProfilePicture { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        // Navigation properties
        public virtual ICollection<Student_Assignment> Student_Assignment { get; set; } = new HashSet<Student_Assignment>();
        public virtual ICollection<Student_Exam> Student_Exam { get; set; } = new HashSet<Student_Exam>();
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public virtual ICollection<student_Course> student_Course { get; set; } = new HashSet<student_Course>();
        public virtual ICollection<student_answers> Answers { get; set; } = new HashSet<student_answers>();
        public virtual ICollection<assignment_Answer> assignment_Answer { get; set; } = new HashSet<assignment_Answer>();

    }
}
