using AutoMapper;
using Business_logic_layer.interfaces;
using Business_logic_layer.Repository;
using Data_access_layer.model;
using Educational_Platform.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Educational_Platform.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly IunitofWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;


        public StudentController(IunitofWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager;
        }

        public async Task<IActionResult> StudentProfile()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if no user is logged in
            }

            // Ensure the user is linked to a student profile
            if (user.StudentId == null)
            {
                return NotFound("Student profile not found.");
            }

            // Retrieve student information
            var student = await _unitOfWork.Student.GetByIdAsync(user.StudentId.Value);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Retrieve courses the student is enrolled in
            var studentCourses = await _unitOfWork.student_CourseRepo
                .GetAllAsync(e => e.StudentID == student.ID, includeProperties: "Course");

            // Map data to the StudentProfileViewModel
            var studentProfileViewModel = new StudentProfileViewModel
            {
                Id = student.ID,
                Email = student.Email,
                FirstName = student.Name,
                PhoneNumber = student.PhoneNumber,
                FatherPhone = student.FatherPhone,
                GradeLevel = student.GradeLevel,
                CurrentProfilePicture = student.ProfilePicture,
                EnrolledCourses = studentCourses.Select(e => new CourseViewModel
                {
                    ID = e.Course.ID,
                    Title = e.Course.Title,
                    Description = e.Course.Description,
                    Duration = e.Course.Duration,
                    Price = e.Course.Price,
                    Image = e.Course.Image
                }).ToList()
            };

            return View(studentProfileViewModel);
        }





        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Charts()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
