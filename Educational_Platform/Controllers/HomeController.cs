using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Educational_Platform.Models;
using Business_logic_layer.interfaces;
using Data_access_layer.model;
using System.Security.Claims;
using Educational_Platform.ViewModel;

namespace Educational_Platform.Controllers;

public class HomeController : Controller
{
    private readonly IunitofWork _unitOfWork;

    public HomeController(IunitofWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    private async Task<bool> IsStudentEnrolledInCourse(int courseId)
    {
    if (User.Identity.IsAuthenticated)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _unitOfWork.Student.FindFirstAsync(s => s.UserId == userId);
        if (user != null)
        {
            var existingEnrollment = await _unitOfWork.student_CourseRepo.FindFirstAsync(sc => sc.StudentID == user.ID && sc.CourseID == courseId);
            return existingEnrollment != null;
        }
    }
    return false;
    }
    public async Task<IActionResult> Index()
    {
        var courses = await _unitOfWork.Course.GetAllAsync();
var courseViewModels = new List<CourseViewModel>();

foreach (var course in courses)
{
    var isEnrolled = await IsStudentEnrolledInCourse(course.ID);
    courseViewModels.Add(new CourseViewModel
    {
        ID = course.ID,
        Title = course.Title,
        Description = course.Description,
        Price = course.Price,
        Duration = course.Duration,
        Image = course.Image,
        status = course.Status, 
        IsEnrolled = isEnrolled
       });
      }

      return View(courseViewModels);
    }
   
    public IActionResult About()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(Message message)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _unitOfWork.Message.AddAsync(message);
                await _unitOfWork.SaveAsync();
                TempData["ContactSuccess"] = "شكرا لتواصلك معنا! لقد تلقينا رسالتك.";
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                TempData["ContactError"] = "حدث خطأ أثناء إرسال رسالتك. يرجى المحاولة مرة أخرى.";
                return RedirectToAction("Contact");
            }
        }

        TempData["ContactError"] = "يرجى ملء جميع الحقول المطلوبة بشكل صحيح.";
        return View(message);
    }
    public async Task<IActionResult> Courses()
    {
        var courses = await _unitOfWork.Course.GetAllAsync();
        var courseViewModels = new List<CourseViewModel>();

        foreach (var course in courses)
        {
            var isEnrolled = await IsStudentEnrolledInCourse(course.ID);
            courseViewModels.Add(new CourseViewModel
            {
                ID = course.ID,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                Duration = course.Duration,
                Image = course.Image,
                status = course.Status,
                IsEnrolled = isEnrolled
            });
        }

        return View(courseViewModels);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
