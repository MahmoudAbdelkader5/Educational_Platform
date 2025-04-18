using Data_access_layer.model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
            public IFormFile videoFile { get; set; } // For uploading video files
            public string SupportingFiles { get; set; }
            public IFormFile Files { get; set; } // For uploading supporting files
            public string TaskFileName { get; set; }
            public IFormFile TaskFile { get; set; }


        public DateTime Create_date { get; set; }
            // Navigation properties
            public int CourseID { get; set; }

            [ForeignKey(nameof(CourseID))]
            public virtual Course Course { get; set; }
        
    }
}
