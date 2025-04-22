using System.ComponentModel.DataAnnotations;

namespace Educational_Platform.ViewModel
{
    public class RegisterStudentViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Father Phone Number")]
        public string FatherPhoneNumber { get; set; }
        [Required]
        [Display(Name = "Grade Level")]
        public string GradeLevel { get; set; }
        // ...existing code...
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePictureFile { get; set; }  // For uploading the file

        [Display(Name = "Profile Picture")]
        public string? ProfilePicture { get; set; } = "default.png";  // For storing the path
        [Required]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
