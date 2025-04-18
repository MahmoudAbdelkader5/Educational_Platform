using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.model
{
    // Course.cs
    public class Course
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]

        [StringLength(50)]
        public string Duration { get; set; }


        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; } = 0.00m;

        public string status { get; set; }

       
        public string Image { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public virtual ICollection<Revision> Revisions { get; set; } = new List<Revision>();
        public virtual ICollection<student_Course> student_Course { get; set; } = new List<student_Course>();
    }
}
