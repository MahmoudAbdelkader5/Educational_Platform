using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Educational_Platform.ViewModel
{
    public class StudentProfileViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Father's Phone")]
        public string FatherPhone { get; set; }

        [Display(Name = "Grade Level")]
        public string GradeLevel { get; set; }

        [Display(Name = "Profile Picture")]
        public string CurrentProfilePicture { get; set; } = "مجهول.png";


        [Display(Name = "Enrolled Courses")]
        public List<CourseViewModel> EnrolledCourses { get; set; }
        public List<ExamResultViewModel> ExamResults { get; set; }
        public List<AssessmentResultViewModel> AssessmentResults { get; set; }

        public double AverageScore { get; set; }
        public int ExamsCount { get; set; }

       
    }
}
