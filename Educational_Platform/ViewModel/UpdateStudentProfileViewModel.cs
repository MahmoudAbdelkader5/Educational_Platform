using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

public class UpdateStudentProfileViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "الاسم مطلوب")]
    [DisplayName("الاسم")]
    public string Name { get; set; }

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
    [DisplayName("البريد الإلكتروني")]
    public string Email { get; set; }
    [Required(ErrorMessage = "رقم الهاتف مطلوب")]

    [DisplayName("رقم الهاتف")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "رقم هاتف الأب  مطلوب")]

    [DisplayName("هاتف الأب")]
    public string FatherPhone { get; set; }
    [Required(ErrorMessage = "الصف الدراسي  مطلوب")]

    [DisplayName("الصف الدراسي")]

    public string GradeLevel { get; set; }

    // Current profile picture path
    [Required(ErrorMessage = "صورة الملف الشخصي  مطلوب")]

    [DisplayName("صورة الملف الشخصي الحالية")]
    public string CurrentProfilePicture { get; set; }

    // New profile picture file
    [Required(ErrorMessage = "صورة الملف الشخصي  مطلوب")]


    [Display(Name = "صورة الملف الشخصي الجديدة")]
    public IFormFile? ProfilePictureFile { get; set; }
}