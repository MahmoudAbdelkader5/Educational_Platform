using System.ComponentModel.DataAnnotations;

namespace Educational_Platform.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل {2} أحرف", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمتا المرور غير متطابقتين")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
