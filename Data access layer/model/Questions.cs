using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_access_layer.model
{
    public class Questions
    {
        [Key]
        public int QuestionID { get; set; }

        [Required]
        public string QuestionText { get; set; }
       
        public string q1 { get; set; }
        public string q2 { get; set; }
        public string q3 { get; set; }
        public string q4 { get; set; }
        public string Answer { get; set; } // Ensure this property is correctly mapped in the database
        public virtual ICollection<assignment_question> Assignment_Questions { get; set; } = new HashSet<assignment_question>();

        public virtual ICollection<assignment_Answer> assignment_Answer { get; set; } = new HashSet<assignment_Answer>();

        public virtual ICollection<examQuestion> ExamQuestions { get; set; } = new HashSet<examQuestion>();
        public virtual ICollection<student_answers> Answers { get; set; } = new HashSet<student_answers>();
    }
}