using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Data_access_layer.model
{
    // Course.cs
    public class Course
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters")]
        [Display(Name = "Course Title")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [StringLength(50, ErrorMessage = "Duration cannot exceed 50 characters")]
        [Display(Name = "Course Duration")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Column(TypeName = "decimal(10, 2)")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00")]
        [DataType(DataType.Currency)]
        [Display(Name = "Course Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Course Status")]
        public string Status { get; set; }

        [Display(Name = "Course Image")]
        public string Image { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}