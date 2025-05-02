using AutoMapper;
using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educational_Platform.Controllers
{
    public class ExamController : Controller
    {
        private readonly IunitofWork _unitOfWork;
        private readonly ILogger<ExamController> _logger;
        private readonly IMapper _mapper;

        public ExamController(
            IunitofWork unitOfWork,
            ILogger<ExamController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: Exam
        public async Task<IActionResult> Index()
        {
            try
            {
                var exams = await _unitOfWork.Exam.GetAllAsync(includeProperties: "Course");
                ViewBag.ExamCount = await _unitOfWork.Exam.GetCountAsync();
                ViewBag.QuestionCount = await _unitOfWork.questions.GetCountAsync();
                await SetViewDataCounts();


                return View(exams);
            }
            catch (Exception ex)
            {
                return View(new List<Exam>());
            }
        }
        private async Task SetViewDataCounts()
        {
            ViewData["messagesCount"] = await _unitOfWork.Message.GetCountAsync();
            ViewData["StudentCount"] = await _unitOfWork.student_CourseRepo.GetCountAsync();
            ViewData["LessonCount"] = await _unitOfWork.Lesson.GetCountAsync();
            ViewData["RevisionCount"] = await _unitOfWork.Lesson.GetCountAsync();
            ViewData["questionCount"] = await _unitOfWork.questions.GetCountAsync();
            ViewData["CourseCount"] = await _unitOfWork.Course.GetCountAsync();
        }
        // GET: Exam/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                await SetViewDataCounts();

                var exam = await _unitOfWork.Exam.GetFirstOrDefaultAsync(
                    filter: e => e.Id == id,
                    includeProperties: "Course");

                if (exam == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var examQuestions = await _unitOfWork.ExamQuestion.GetQuestionsForExamAsync(id);
                ViewBag.ExamQuestions = examQuestions;
                return View(exam);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Exam/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                await SetViewDataCounts();

                await PopulateCreateViewBags();
                return View(new Exam());
            }
            catch (Exception ex)
            {
                
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Exam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam, List<int> selectedQuestionIds)
        {
            try
            {
                await SetViewDataCounts();

                await PopulateCreateViewBags();

                await _unitOfWork.Exam.AddAsync(exam);
                await _unitOfWork.Save();

                if (selectedQuestionIds != null && selectedQuestionIds.Any())
                {
                    foreach (var questionId in selectedQuestionIds)
                    {
                        await _unitOfWork.ExamQuestion.AddAsync(new examQuestion
                        {
                            ExamID = exam.Id,
                            QuestionID = questionId,
                            mark = 1 // Default mark
                        });
                    }
                    await _unitOfWork.Save();
                }

                return RedirectToAction(nameof(Details), new { id = exam.Id });
            }
            catch (Exception ex)
            {
                
                await PopulateCreateViewBags();
                return View(exam);
            }
        }

        // GET: Exam/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                await SetViewDataCounts();
                var exam = await _unitOfWork.Exam.GetFirstOrDefaultAsync(
                    e => e.Id == id,
                    includeProperties: "ExamQuestions.Question");

                if (exam == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                await PopulateEditViewBags(exam);
                return View(exam);
            }
            catch (Exception ex)
            {
                
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Exam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exam exam, List<int> selectedQuestionIds)
        {
            if (id != exam.Id)
            {
                TempData["ErrorMessage"] = "Exam ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await SetViewDataCounts();

                await PopulateEditViewBags(exam);

                // Update exam
                _unitOfWork.Exam.UpdateAsync(exam);

                // Handle exam questions
                await UpdateExamQuestions(id, selectedQuestionIds);

                await _unitOfWork.Save();

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating exam with ID {id}");
                await PopulateEditViewBags(exam);
                return View(exam);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await SetViewDataCounts();
                var exam = await _unitOfWork.Exam.GetByIdAsync(id);
                if (exam == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Delete related exam questions first
                var examQuestions = await _unitOfWork.ExamQuestion.GetQuestionsForExamAsync(id);
                foreach (var eq in examQuestions)
                {
                     _unitOfWork.ExamQuestion.DeleteAsync(eq);
                }

                 _unitOfWork.Exam.DeleteAsync(exam);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting exam with ID {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Exam/ManageQuestions/5
        public async Task<IActionResult> ManageQuestions(int id)
        {
            try
            {
                var exam = await _unitOfWork.Exam.GetByIdAsync(id);
                if (exam == null)
                {
                    TempData["ErrorMessage"] = "Exam not found.";
                    return RedirectToAction(nameof(Index));
                }

                var examQuestions = await _unitOfWork.ExamQuestion.GetQuestionsForExamAsync(id);
                ViewBag.Exam = exam;
                return View(examQuestions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving questions for exam with ID {id}");
                TempData["ErrorMessage"] = "An error occurred while retrieving exam questions.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Exam/UpdateQuestionMark/5
        [HttpPost]
        public async Task<IActionResult> UpdateQuestionMark(int examQuestionId, decimal mark)
        {
            try
            {
                var examQuestion = await _unitOfWork.ExamQuestion.GetByIdAsync(examQuestionId);
                if (examQuestion == null)
                {
                    return Json(new { success = false, message = "Exam question not found." });
                }

                examQuestion.mark = mark;
                _unitOfWork.ExamQuestion.UpdateAsync(examQuestion);
                await _unitOfWork.Save();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating mark for exam question with ID {examQuestionId}");
                return Json(new { success = false, message = "An error occurred while updating the mark." });
            }
        }

        #region Helper Methods

        private async Task PopulateCreateViewBags()
        {
            ViewBag.Questions = await _unitOfWork.questions.GetAllAsync();
            ViewBag.Courses = await _unitOfWork.Course.GetAllAsync();
        }

        private async Task PopulateEditViewBags(Exam exam)
        {
            ViewBag.AllQuestions = await _unitOfWork.questions.GetAllAsync();
            ViewBag.Courses = await _unitOfWork.Course.GetAllAsync();
            ViewBag.SelectedQuestionIds = exam.ExamQuestions?
                .Select(eq => eq.QuestionID)
                .ToList() ?? new List<int>();
        }

        private async Task UpdateExamQuestions(int examId, List<int> selectedQuestionIds)
        {
            var existingExamQuestions = await _unitOfWork.ExamQuestion.GetAllAsync(
                eq => eq.ExamID == examId);

            // Remove unselected questions
            if (existingExamQuestions.Any())
            {
                var questionsToRemove = existingExamQuestions
                    .Where(eq => selectedQuestionIds == null || !selectedQuestionIds.Contains(eq.QuestionID))
                    .ToList();

                foreach (var question in questionsToRemove)
                {
                    _unitOfWork.ExamQuestion.DeleteAsync(question);
                }
            }

            // Add new questions
            if (selectedQuestionIds != null)
            {
                var existingQuestionIds = existingExamQuestions
                    .Select(eq => eq.QuestionID)
                    .ToHashSet();

                var questionsToAdd = selectedQuestionIds
                    .Where(qId => !existingQuestionIds.Contains(qId))
                    .Select(qId => new examQuestion
                    {
                        ExamID = examId,
                        QuestionID = qId,
                        mark = 1 // Default mark
                    });

                foreach (var question in questionsToAdd)
                {
                    await _unitOfWork.ExamQuestion.AddAsync(question);
                }
            }
        }

        #endregion
    }
}