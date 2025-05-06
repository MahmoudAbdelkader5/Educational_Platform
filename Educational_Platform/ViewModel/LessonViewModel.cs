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

        [Required(ErrorMessage = "مطلوب عنوان الدرس")]
        [StringLength(255, ErrorMessage = "يجب أن يكون العنوان أقل من 255 حرفًا")]
        [Display(Name = "عنوان الدرس")]
        public string Title { get; set; }

        [StringLength(255, ErrorMessage = "يجب أن يكون رابط الفيديو أقل من 255 حرفًا")]
        [Display(Name = "رابط الفيديو")]
        public string VideoURL { get; set; }


        [Display(Name = "الملفات المرفقة")]
        [Required(ErrorMessage = "مطلوب الملفات الدرس")]

        public string SupportingFiles { get; set; }

        [Display(Name = "اسم ملف المهمة")]
        [Required(ErrorMessage = "مطلوب ملف  المهمةالدرس")]

        public string TaskFileName { get; set; }

        [Display(Name = "تاريخ الإنشاء")]
        [Required(ErrorMessage = "مطلوب تاريخ الدرس")]

        public DateTime Create_date { get; set; }

        [Required(ErrorMessage = "مطلوب معرف الدورة")]
        [Display(Name = "معرف الدورة")]
        public int CourseID { get; set; }
        [Required(ErrorMessage = "مطلوب اسم الكورس ")]

        // ارتباط الدورة
        [ForeignKey("CourseID")]
        public Course Course { get; set; }

        // ملفات غير مرئية (ملفات تحميل)
        [Required(ErrorMessage = "مطلوب فيديو الدرس")]

        [NotMapped]
        [Display(Name = "ملف الفيديو")]
        
        public IFormFile videoFile { get; set; }

        [NotMapped]
        [Display(Name = "ملف المهمة")]
        [Required(ErrorMessage = "مطلوب ملف الدرس")]

        public IFormFile TaskFile { get; set; }

        [NotMapped]
        [Display(Name = "ملف ")]
        public IFormFile Files { get; set; }
    }
}