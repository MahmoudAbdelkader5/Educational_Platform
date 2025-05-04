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
        public int Id { get; set; }  // Changed from ID to Id for consistency

        [ForeignKey(nameof(Course))]
        [Display(Name = "Course")]
        public int? CourseId { get; set; }

        [Required(ErrorMessage = "Exam title is required")]
        [StringLength(255, MinimumLength = 5,
            ErrorMessage = "Title must be between 5 and 255 characters")]
        [Display(Name = "Exam Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 480, ErrorMessage = "Duration must be between 1 and 480 minutes")]
        [Display(Name = "Duration (minutes)")]
        public int duration { get; set; }  // Changed to PascalCase

        // Navigation properties
        public virtual Course Course { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "At least one question is required")]
        [MinLength(1, ErrorMessage = "At least one question is required")]
        [ValidateAtLeastOneQuestion(ErrorMessage = "You must select at least one question")]

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

        return new ValidationResult(ErrorMessage ?? "You must select at least one question");
    }
}

