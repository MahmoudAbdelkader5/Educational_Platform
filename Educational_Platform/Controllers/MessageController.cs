using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Educational_Platform.Controllers
{
    [Authorize(Roles = "Instructor")]

    public class MessageController : Controller
    {
        private readonly IunitofWork _unitOfWork;

        public MessageController(IunitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Message> messages;
            ViewData["messagesCount"] = await _unitOfWork.Message.GetCountAsync();
            ViewData["StudentCount"] = await _unitOfWork.student_CourseRepo.GetCountAsync();
            ViewData["LessonCount"] = await _unitOfWork.Lesson.GetCountAsync();
            ViewData["RevisionCount"] = await _unitOfWork.Lesson.GetCountAsync();
            ViewData["questionCount"] = await _unitOfWork.questions.GetCountAsync();
            ViewData["CourseCount"] = await _unitOfWork.Course.GetCountAsync();
            messages = await _unitOfWork.Message.GetAllAsync();

            return View(messages);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.Message.GetByIdAsync(id);
            if (message == null)
            {
                TempData["error"] = "الرسالة غير موجودة.";
                return RedirectToAction("Index");
            }

            _unitOfWork.Message.DeleteAsync(message);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "تم حذف الرسالة بنجاح.";
            return RedirectToAction("Index");
        }

    }
}
