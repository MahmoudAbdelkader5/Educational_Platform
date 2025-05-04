using System.ComponentModel.DataAnnotations;

namespace Educational_Platform.ViewModel
{
    public class RegisterStudentViewModel
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الأول يجب أن لا يتجاوز {1} حرف")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم الأخير يجب أن لا يتجاوز {1} حرف")]
        [Display(Name = "الاسم الأخير")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "رقم هاتف الأب مطلوب")]
        [Display(Name = "رقم هاتف الأب")]
        public string FatherPhoneNumber { get; set; }

        [Required(ErrorMessage = "الصف الدراسي مطلوب")]
        [Display(Name = "الصف الدراسي")]
        public string GradeLevel { get; set; }

        [Display(Name = "صورة الملف الشخصي")]
        public IFormFile? ProfilePictureFile { get; set; }  // For uploading the file

        [Display(Name = "صورة الملف الشخصي")]
        public string? ProfilePicture { get; set; } = "مجهول.png";  // For storing the path

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, ErrorMessage = "يجب أن تكون كلمة المرور على الأقل {2} أحرف", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; }
    }
}