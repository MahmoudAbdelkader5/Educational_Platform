using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_access_layer.model
{
    public class Questions
    {
        [Key]
        public int QuestionID { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        [StringLength(1000, MinimumLength = 10,
    ErrorMessage = "Question must be between 10 and 1000 characters")]
        [Display(Name = "Question Text")]
        public string QuestionText { get; set; }
        [Required(ErrorMessage = "Option 1 is required")]
        [StringLength(500, MinimumLength = 1,
     ErrorMessage = "Option must be between 1 and 500 characters")]
        [Display(Name = "Option 1")]
        public string q1 { get; set; }
        [Required(ErrorMessage = "Option 2 is required")]
        [StringLength(500, MinimumLength = 1,
    ErrorMessage = "Option must be between 1 and 500 characters")]
        [Display(Name = "Option 2")]
        public string q2 { get; set; }
       [ Required(ErrorMessage = "Option 3 is required")]
[StringLength(500, MinimumLength = 1,
    ErrorMessage = "Option must be between 1 and 500 characters")]
        [Display(Name = "Option 3")]
        public string q3 { get; set; }
        [Required(ErrorMessage = "Option 4 is required")]
        [StringLength(500, MinimumLength = 1,
    ErrorMessage = "Option must be between 1 and 500 characters")]
        [Display(Name = "Option 4")]
        public string q4 { get; set; }
        public string Answer { get; set; } // Ensure this property is correctly mapped in the database
        public virtual ICollection<assignment_question> Assignment_Questions { get; set; } = new HashSet<assignment_question>();

        public virtual ICollection<assignment_Answer> assignment_Answer { get; set; } = new HashSet<assignment_Answer>();

        public virtual ICollection<examQuestion> ExamQuestions { get; set; } = new HashSet<examQuestion>();
        public virtual ICollection<student_answers> Answers { get; set; } = new HashSet<student_answers>();
    }
}