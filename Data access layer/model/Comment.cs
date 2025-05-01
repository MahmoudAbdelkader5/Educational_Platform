using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.model
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }

        public int LessonID { get; set; }
        public int? StudentID { get; set; }
        public int? InstructorID { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CommentDate { get; set; } 

        public string? Reply { get; set; }

        // Navigation properties
        [ForeignKey(nameof(LessonID))]
        public virtual Lesson Lesson { get; set; }

        [ForeignKey(nameof(StudentID))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(InstructorID))]
        public virtual Instructor Instructor { get; set; }
    }
}
