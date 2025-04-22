using AutoMapper;
using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Educational_Platform.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educational_Platform.Controllers
{

    public class CourseController : Controller
    {
        private readonly IunitofWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public IMapper Mapper { get; }

        public CourseController(IunitofWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        [Authorize(Roles = "Instructor")]

        public async Task<IActionResult> Index(string search)
        {
            IEnumerable<Course> courses;

            if (string.IsNullOrEmpty(search))
            {
                courses = await _unitOfWork.Course.GetAllAsync();
            }
            else
            {
                courses =  _unitOfWork.Course.searchCourseBytitle(search); 
            }
            var courseCount = courses.Count();
            ViewData["CourseCount"] = courseCount;

            if (courses == null || !courses.Any())
            {
                TempData["Message"] = "No Courses found.";
                return View(new List<CourseViewModel>());
            }

            var mappedCourses = Mapper.Map<IEnumerable<CourseViewModel>>(courses);
            return View(mappedCourses);
        }

        [HttpGet]
        [Authorize(Roles = "Instructor")]

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create(CourseViewModel courseVm)
        {
            try
            {
                var courses = Mapper.Map<Course>(courseVm);

                if (courseVm.ImageFile != null)
                {
                    courses.Image = Helper.Helper.uploadfile(courseVm.ImageFile, "imgCourse");
                }

                await _unitOfWork.Course.AddAsync(courses);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = $"course '{courses.Title}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the course.";
                return View(courseVm);
            }


        }
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _unitOfWork.Course.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var courseViewModel = Mapper.Map<CourseViewModel>(course);
            return View(courseViewModel);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DetailsWithLessons(int id)
        {
            var course = await _unitOfWork.Course.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var lessons = await _unitOfWork.Lesson.GetAllAsync();
            var courseLessons = lessons.Where(l => l.CourseID == id);

            var courseDetailsViewModel = new CourseDetailsViewModel
            {
                ID = course.ID,
                Title = course.Title,
                Description = course.Description,
                Duration = course.Duration,
                Image = course.Image, 
                Lessons = courseLessons.Select(l => new LessonViewModel
                {
                    ID = l.ID,
                    Title = l.Title,
                    VideoURL = l.VideoURL, 
                    SupportingFiles = l.SupportingFiles,
                    TaskFileName = l.TaskFileName,
                    Create_date = l.Create_date,
                }).ToList()
            };

            return View(courseDetailsViewModel); 
        }


        [Authorize(Roles = "Instructor")]

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }

            var course = await _unitOfWork.Course.GetByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            var mappedCourse = Mapper.Map<CourseViewModel>(course);
            return View(mappedCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]

        public async Task<IActionResult> Edit(CourseViewModel course)
        {
            try
            {
                var courses = Mapper.Map<Course>(course);

                if (course.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(courses.Image))
                    {
                        Helper.Helper.deletefile(courses.Image, "imgCourse");
                    }
                    courses.Image = Helper.Helper.uploadfile(course.ImageFile, "imgCourse");
                }



                _unitOfWork.Course.UpdateAsync(courses);
                var res = await _unitOfWork.Save();

                if (res > 0)
                {
                    TempData["SuccessMessage"] = $"Course '{courses.Title}' updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                return View(course);
                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the course.";
                return View(course);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _unitOfWork.Course.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            try
            {
                var mappedCourse = Mapper.Map<Course>(course);
                 _unitOfWork.Course.DeleteAsync(mappedCourse); // Await this async operation  
                var res = await _unitOfWork.Save();

                if (res > 0 && mappedCourse.Image != null)
                {
                    Helper.Helper.deletefile(mappedCourse.Image, "imgCourse");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting course: " + ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EnrollInCourse(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "يرجى تسجيل الدخول أولاً";
                return RedirectToAction("Login", "Account");
            }

            if (user.StudentId == null)
            {
                TempData["Error"] = "لم يتم العثور على ملف الطالب";
                return RedirectToAction("StudentProfile", "Student");
            }

            // Retrieve the student and course
            var student = await _unitOfWork.Student.GetByIdAsync(user.StudentId.Value);
            var course = await _unitOfWork.Course.GetByIdAsync(id);

            if (student == null || course == null)
            {
                TempData["Error"] = "لم يتم العثور على الطالب أو الكورس";
                return RedirectToAction("StudentProfile", "Student");
            }

            var existingEnrollment = (await _unitOfWork.student_CourseRepo
                .GetAllAsync(e => e.StudentID == student.ID && e.CourseID == course.ID))
                .FirstOrDefault();

            if (existingEnrollment != null)
            {
                TempData["Error"] = "أنت بالفعل مشترك في هذا الكورس";
                return RedirectToAction("StudentProfile", "Student");
            }

            var enrollment = new student_Course
            {
                StudentID = student.ID,
                CourseID = course.ID,
                PaymentStatus = "Paid"
            };

            await _unitOfWork.student_CourseRepo.AddAsync(enrollment);
            await _unitOfWork.Save();

            TempData["Success"] = "لقد اشتركت في هذا الكورس بنجاح";
            return RedirectToAction("StudentProfile", "Student");
        }

      

    }

}
