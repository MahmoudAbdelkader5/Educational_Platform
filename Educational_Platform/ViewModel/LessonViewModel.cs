using Data_access_layer.model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Educational_Platform.ViewModel
{
    public class LessonViewModel
    {

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(255)]
        public string VideoURL { get; set; }
        public string SupportingFiles { get; set; }
        public string TaskFileName { get; set; }
        public DateTime Create_date { get; set; }

        [Required]
        public int CourseID { get; set; }

        [ForeignKey("CourseID")]
        public Course Course { get; set; }

        [NotMapped]
        public IFormFile videoFile { get; set; }

        [NotMapped]
        public IFormFile TaskFile { get; set; }

        [NotMapped]
        public IFormFile Files { get; set; }

    }
}
