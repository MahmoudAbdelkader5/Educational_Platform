using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Educational_Platform.ViewModel
{
    public class CourseViewModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "مطلوب عنوان الدورة.")]
        [StringLength(255, ErrorMessage = "يجب أن يكون العنوان أقل من 255 حرفًا.")]
        [Display(Name = "عنوان الدورة")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "الوصف")]
        public string Description { get; set; }

        [Required(ErrorMessage = "مطلوب مدة الدورة.")]
        [StringLength(50, ErrorMessage = "يجب أن تكون المدة أقل من 50 حرفًا.")]
        [Display(Name = "المدة")]
        public string Duration { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 10000, ErrorMessage = "يجب أن يكون السعر بين 0 و 10,000.")]
        [Display(Name = "السعر")]
        public decimal Price { get; set; } = 0.00m;

        [Required(ErrorMessage = "مطلوب الحالة.")]
        [Display(Name = "الحالة")]
        public string status { get; set; }

        // For displaying existing image (if editing)
        [Required(ErrorMessage = "مطلوب الصوره.")]

        public string Image { get; set; }

        // For file upload

        public IFormFile? ImageFile { get; set; }

        public bool? IsEnrolled { get; set; } = false;
    }
}