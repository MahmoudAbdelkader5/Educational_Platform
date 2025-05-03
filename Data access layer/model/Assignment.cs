using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_access_layer.model
{
    public class Assignment
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }



        public int duration { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal MaxGrade { get; set; }


        // Navigation properties
        public int CourseID { get; set; }
        public int? LessonID { get; set; }

        [ForeignKey(nameof(CourseID))]
        public virtual Course Course { get; set; }

        [ForeignKey(nameof(LessonID))]
        public virtual Lesson Lesson { get; set; }
        public virtual ICollection<assignment_question> assignment_Question { get; set; } = new HashSet<assignment_question>();
        public virtual ICollection<Student_Assignment> Student_Assignment { get; set; } = new List<Student_Assignment>();
    }


}
