using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_access_layer.model
{
    public class Questions
    {
        [Key]
        public int QuestionID { get; set; }

        [Required(ErrorMessage = "نص السؤال مطلوب")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "يجب أن يتراوح نص السؤال بين 10 و 1000 حرف")]
        [Display(Name = "نص السؤال")]
        public string QuestionText { get; set; }

        [Required(ErrorMessage = "الخيار 1 مطلوب")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "يجب أن يتراوح طول الخيار بين 1 و 500 حرف")]
        [Display(Name = "الخيار 1")]
        public string q1 { get; set; }

        [Required(ErrorMessage = "الخيار 2 مطلوب")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "يجب أن يتراوح طول الخيار بين 1 و 500 حرف")]
        [Display(Name = "الخيار 2")]
        public string q2 { get; set; }

        [Required(ErrorMessage = "الخيار 3 مطلوب")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "يجب أن يتراوح طول الخيار بين 1 و 500 حرف")]
        [Display(Name = "الخيار 3")]
        public string q3 { get; set; }

        [Required(ErrorMessage = "الخيار 4 مطلوب")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "يجب أن يتراوح طول الخيار بين 1 و 500 حرف")]
        [Display(Name = "الخيار 4")]
        public string q4 { get; set; }
        [Required(ErrorMessage = "مطلوب الاجابه السوال")]


        // تأكد من أن هذا الحقل يسمى بشكل صحيح في قاعدة البيانات
        public string Answer { get; set; }

        public virtual ICollection<assignment_question> Assignment_Questions { get; set; } = new HashSet<assignment_question>();

        public virtual ICollection<assignment_Answer> assignment_Answer { get; set; } = new HashSet<assignment_Answer>();

        public virtual ICollection<examQuestion> ExamQuestions { get; set; } = new HashSet<examQuestion>();
        public virtual ICollection<student_answers> Answers { get; set; } = new HashSet<student_answers>();
    }
}