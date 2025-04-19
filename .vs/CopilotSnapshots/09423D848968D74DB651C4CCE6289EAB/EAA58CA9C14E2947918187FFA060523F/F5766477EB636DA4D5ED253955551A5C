using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Educational_Platform.ViewModel
{
    public class CourseViewModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Course title is required.")]
        [StringLength(255, ErrorMessage = "Title must be less than 255 characters.")]
        [Display(Name = "Course Title")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [StringLength(50, ErrorMessage = "Duration must be less than 50 characters.")]
        [Display(Name = "Duration")]
        public string Duration { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 10000, ErrorMessage = "Price must be between $0 and $10,000.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; } = 0.00m;

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string status { get; set; }

        // For displaying existing image (if editing)
        public string Image { get; set; }

        // For file upload
        
        public IFormFile ImageFile { get; set; }
    }

    // Custom validation attribute for file extensions
    
    
}