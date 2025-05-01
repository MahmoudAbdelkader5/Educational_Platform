using System.ComponentModel.DataAnnotations;

public class UpdateStudentProfileViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string PhoneNumber { get; set; }
    public string FatherPhone { get; set; }
    public string GradeLevel { get; set; }

    // Current profile picture path
    public string CurrentProfilePicture { get; set; }

    // New profile picture file
    [Display(Name = "Profile Picture")]
    public IFormFile? ProfilePictureFile { get; set; }
}