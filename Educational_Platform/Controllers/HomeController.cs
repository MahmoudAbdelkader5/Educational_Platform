using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Educational_Platform.Models;
using Business_logic_layer.interfaces;
using Data_access_layer.model;

namespace Educational_Platform.Controllers;

public class HomeController : Controller
{
    private readonly IunitofWork _unitOfWork;

    public HomeController(IunitofWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<Course> courses;  
        courses = await _unitOfWork.Course.GetAllAsync();
        
        return View(courses);
    }
   
    public IActionResult About()
    {
        return View();
    }
    public IActionResult Contact()
    {
        return View();
    }
    public async Task<IActionResult> Courses()
    {
        IEnumerable<Course> courses;
        courses = await _unitOfWork.Course.GetAllAsync();

        return View(courses);
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
