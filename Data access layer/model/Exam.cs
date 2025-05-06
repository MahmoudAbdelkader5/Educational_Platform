using Data_access_layer.model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_access_layer.model
{
    public class Exam
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "المادة الدراسية مطلوب")]

        [ForeignKey(nameof(Course))]
        [Display(Name = "المادة الدراسية")]
        public int? CourseId { get; set; }

        [Required(ErrorMessage = "عنوان الامتحان مطلوب")]
        [StringLength(255, MinimumLength = 5,
            ErrorMessage = "يجب أن يكون العنوان بين 5 و 255 حرفًا")]
        [Display(Name = "عنوان الامتحان")]
        public string Title { get; set; }

        [Required(ErrorMessage = "مدة الامتحان مطلوبة")]
        [Range(1, 480, ErrorMessage = "يجب أن تكون المدة بين 1 و 480 دقيقة")]
        [Display(Name = "المدة (بالدقائق)")]
        public int duration { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "المادة الدراسية مطلوب")]

        public virtual Course Course { get; set; }

        [Required(ErrorMessage = "يجب إضافة سؤال واحد على الأقل")]
        [MinLength(1, ErrorMessage = "يجب إضافة سؤال واحد على الأقل")]
        [ValidateAtLeastOneQuestion(ErrorMessage = "يجب اختيار سؤال واحد على الأقل")]
        public virtual ICollection<examQuestion> ExamQuestions { get; set; } = new HashSet<examQuestion>();

        public virtual ICollection<Student_Exam> StudentExams { get; set; } = new HashSet<Student_Exam>();
    }

}
public class ValidateAtLeastOneQuestion : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is ICollection<examQuestion> questions && questions.Count > 0)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "يجب اختيار سؤال واحد على الأقل");
    }
}

