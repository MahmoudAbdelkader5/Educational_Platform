using Data_access_layer.model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Educational_Platform.ViewModel
{
    public class RevisionModelView
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "مطلوب اسم الكورس ")]

        public int CourseID { get; set; }

        [Required(ErrorMessage = "العنوان مطلوب")]
        [StringLength(255, ErrorMessage = "يجب أن يكون العنوان أقل من 255 حرفًا")]
        [Display(Name = "العنوان")]
        public string Title { get; set; }

        [StringLength(255, ErrorMessage = "يجب أن يكون عنوان الفيديو أقل من 255 حرفًا")]
        [Display(Name = "الفيديو")]
        public string Video { get; set; }
        [Required(ErrorMessage = "مطلوب الملف المراجعه")]


        [Display(Name = "الملفات")]
        public string Files { get; set; }

        [Required(ErrorMessage = "تاريخ الجدولة مطلوب")]
        [Display(Name = "تاريخ الجدولة")]
        public DateTime ScheduleDate { get; set; }

        [Display(Name = "ملف المراجعة")]
        [Required(ErrorMessage = "مطلوب ملف المراجعه")]

        public IFormFile File { get; set; }
        [Required(ErrorMessage = "مطلوب فيديو المراجعه")]


        [Display(Name = "ملف الفيديو")]
        public IFormFile VideoFile { get; set; }
        [Required(ErrorMessage = "مطلوب اسم الكورس ")]

        // خصائص الربط
        [ForeignKey(nameof(CourseID))]
        public virtual Course Course { get; set; }
    }
}