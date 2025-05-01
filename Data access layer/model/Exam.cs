using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_access_layer.model
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }  // Changed from ID to Id for consistency

        [ForeignKey(nameof(Course))]
        public int ? CourseId { get; set; }  // Changed from CourseID to CourseId

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

       

        public int duration { get; set; }




        // Navigation properties
        public virtual Course Course { get; set; }

        public virtual ICollection<examQuestion> ExamQuestions { get; set; } = new HashSet<examQuestion>();  

        public virtual ICollection<Student_Exam> StudentExams { get; set; } = new HashSet<Student_Exam>();  
    }
}