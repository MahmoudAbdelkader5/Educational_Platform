using AutoMapper;
using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Educational_Platform.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IunitofWork _unitOfWork;
        private readonly ILogger<QuestionController> _logger;

        public QuestionController(IunitofWork unitOfWork, ILogger<QuestionController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Question
        public async Task<IActionResult> Index()
        {
            try
            {
                var questions = await _unitOfWork.questions.GetAllAsync();
                ViewData["questionCount"] = await _unitOfWork.questions.GetCountAsync();
                ViewData["CourseCount"] = await _unitOfWork.Course.GetCountAsync();

                ViewData["StudentCount"] = await _unitOfWork.student_CourseRepo.GetCountAsync();
                ViewData["LessonCount"] = await _unitOfWork.Lesson.GetCountAsync();
                ViewData["RevisionCount"] = await _unitOfWork.Lesson.GetCountAsync();

                return View(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving questions");
                TempData["ErrorMessage"] = "An error occurred while retrieving questions.";
                return View(Array.Empty<Questions>());
            }
        }

        // GET: Question/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Question ID not provided.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var question = await _unitOfWork.questions.GetByIdAsync(id.Value);
                if (question == null)
                {
                    TempData["ErrorMessage"] = "Question not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving question with ID {id}");
                TempData["ErrorMessage"] = "An error occurred while retrieving the question.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Question/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Questions question)
        {
            if (!ModelState.IsValid)
            {
                return View(question);
            }

            try
            {
                await _unitOfWork.questions.AddAsync(question);
                var res = await _unitOfWork.Save();

                if (res > 0)
                {
                    TempData["SuccessMessage"] = "Question created successfully";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Failed to create question.";
                return View(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating question");
                TempData["ErrorMessage"] = "An error occurred while creating the question.";
                return View(question);
            }
        }

        // GET: Question/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var question = await _unitOfWork.questions.GetByIdAsync(id);
                if (question == null)
                {
                    TempData["ErrorMessage"] = "Question not found.";
                    return RedirectToAction(nameof(Index));
                }
                return View(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving question with ID {id} for edit");
                TempData["ErrorMessage"] = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Questions question)
        {
            if (id != question.QuestionID)
            {
                TempData["ErrorMessage"] = "Question ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(question);
            }

            try
            {
                var existingQuestion = await _unitOfWork.questions.GetByIdAsync(id);
                if (existingQuestion == null)
                {
                    TempData["ErrorMessage"] = "Question not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Update properties
                existingQuestion.QuestionText = question.QuestionText;
                existingQuestion.q1 = question.q1;
                existingQuestion.q2 = question.q2;
                existingQuestion.q3 = question.q3;
                existingQuestion.q4 = question.q4;
                existingQuestion.Answer = question.Answer;

                _unitOfWork.questions.UpdateAsync(existingQuestion);
                var result = await _unitOfWork.Save();

                if (result > 0)
                {
                    TempData["SuccessMessage"] = $"Question updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Failed to update question.";
                return View(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating question with ID {id}");
                TempData["ErrorMessage"] = "An error occurred while updating the question.";
                return View(question);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _unitOfWork.questions.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            try
            {
                _unitOfWork.questions.DeleteAsync(employee);
                var res = await _unitOfWork.Save();

                if (res > 0 )
                {
                    TempData["Message"] = $"questions deleted: {employee.QuestionText}";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting questions: " + ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}