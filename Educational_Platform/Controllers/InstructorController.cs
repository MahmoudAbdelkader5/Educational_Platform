using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Educational_Platform.Models;  // To access the Instructor class

namespace Educational_Platform.Controllers
{
    public class InstructorController : Controller
    {
        //private readonly IunitofWork _unitOfWork;
        //private readonly UserManager<ApplicationUser> _userManager;

        //public InstructorController(IunitofWork unitOfWork, UserManager<ApplicationUser> userManager)
        //{
        //    _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        //    _userManager = userManager;

        //}
        //public async Task<IActionResult> Index()
        //{
        //    if (_unitOfWork == null)
        //        throw new Exception("_unitOfWork is null!");

        //    if (_unitOfWork.Instructor == null)
        //        throw new Exception("_unitOfWork.Instructor is null!");

        //    var instructors = await _unitOfWork.Instructor.GetAllAsync();

        //    return View(instructors);
        //}
        private readonly ApplicationDbContext _context; // Inject ApplicationDbContext

        public InstructorController(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IActionResult> Index()
        {
            // Directly query the Instructor table
            var instructors = await _context.Instructors.ToListAsync(); // Adjust this to your actual DbSet name

            return View(instructors);
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
