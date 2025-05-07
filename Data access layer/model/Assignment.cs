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

        [Required(ErrorMessage = "عنوان الواجب مطلوب")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "يجب أن يكون العنوان بين 5 و 255 حرفًا")]
        [Display(Name = "عنوان الواجب")]
        public string Title { get; set; }

        [Required(ErrorMessage = "مدة الواجب مطلوبة")]
        [Range(1, 480, ErrorMessage = "يجب أن تكون المدة بين 1 و 480 دقيقة")]
        [Display(Name = "المدة")]
        public int duration { get; set; }

        [Required(ErrorMessage = "الدرجة الكاملة مطلوبة")]
        [Range(0.01, 100.00, ErrorMessage = "يجب أن تكون الدرجة بين 0.01 و 100")]
        [Column(TypeName = "decimal(5, 2)")]
        [Display(Name = "أعلى درجة")]
        public decimal MaxGrade { get; set; }

        // Navigation properties
        [Required(ErrorMessage = "يجب تحديد المادة الدراسية")]
        [Display(Name = "المادة الدراسية")]
        public int CourseID { get; set; }

        [Display(Name = "الدرس")]
        public int? LessonID { get; set; }

        [ForeignKey(nameof(CourseID))]
        [Required(ErrorMessage = "يجب تحديد المادة الدراسية")]
        public virtual Course Course { get; set; }

        [ForeignKey(nameof(LessonID))]
        public virtual Lesson Lesson { get; set; }

        [ValidateAtLeastOneQuestion(ErrorMessage = "يجب اختيار سؤال واحد على الأقل")]
        [Display(Name = "أسئلة الواجب")]
        public virtual ICollection<assignment_question> assignment_Question { get; set; } = new HashSet<assignment_question>();

        [Display(Name = "واجبات الطلاب")]
        public virtual ICollection<Student_Assignment> Student_Assignment { get; set; } = new List<Student_Assignment>();
    }

    public class ValidateAtLeastOneQuestion : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is ICollection<assignment_question> questions && questions.Count > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? "يجب اختيار سؤال واحد على الأقل");
        }
    }
}