using AutoMapper;
using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Educational_Platform.Controllers
{
    public class AssessmentController : Controller
    {
        private readonly IunitofWork _unitOfWork;
        private readonly ILogger<AssessmentController> _logger;
        private readonly IMapper _mapper;

        public AssessmentController(
            IunitofWork unitOfWork,
            ILogger<AssessmentController> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: Assessment
        public async Task<IActionResult> Index()
        {
            try
            {
                var assessments = await _unitOfWork.Assessment.GetAllAsync(includeProperties: "Course,Lesson");
                ViewBag.AssessmentCount = await _unitOfWork.Assessment.GetCountAsync();
                ViewBag.QuestionCount = await _unitOfWork.questions.GetCountAsync();
                await SetViewDataCounts();

                return View(assessments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assessments");
                return View(new List<Assignment>());
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
            ViewData["AssessmentCount"] = await _unitOfWork.Assessment.GetCountAsync();
            ViewData["examCount"] = await _unitOfWork.Exam.GetCountAsync();
        }

        // GET: Assessment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                await SetViewDataCounts();

                var assessment = await _unitOfWork.Assessment.GetFirstOrDefaultAsync(
                    filter: a => a.ID == id,
                    includeProperties: "Course,Lesson,assignment_Question.Question");

                if (assessment == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(assessment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving assessment details with ID {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Assessment/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                await SetViewDataCounts();
                await PopulateCreateViewBags();
                return View(new Assignment());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading assessment creation page");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Assessment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Assignment assessment, List<int> selectedQuestionIds)
        {
            try
            {
                await SetViewDataCounts();

                {
                    await _unitOfWork.Assessment.AddAsync(assessment);
                    await _unitOfWork.Save();

                    if (selectedQuestionIds != null && selectedQuestionIds.Any())
                    {
                        foreach (var questionId in selectedQuestionIds)
                        {
                            await _unitOfWork.AssignmentQuestion.AddAsync(new assignment_question
                            {
                                AssignmentID = assessment.ID,
                                QuestionID = questionId,
                                mark = 1 // Default mark
                            });
                        }
                        await _unitOfWork.Save();
                    }
                    await PopulateCreateViewBags();


                    return RedirectToAction(nameof(Details), new { id = assessment.ID });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new assessment");
                await PopulateCreateViewBags();
                return View(assessment);
            }
        }

        // GET: Assessment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                await SetViewDataCounts();
                var assessment = await _unitOfWork.Assessment.GetFirstOrDefaultAsync(
                    a => a.ID == id,
                    includeProperties: "Course,Lesson,assignment_Question.Question");

                if (assessment == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                await PopulateEditViewBags(assessment);
                return View(assessment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error loading edit page for assessment with ID {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Assignment assessment, List<int> selectedQuestionIds)
        {
            if (id != assessment.ID)
            {
                TempData["ErrorMessage"] = "تعارض في معرّف التقييم";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await SetViewDataCounts();

               
                    _unitOfWork.Assessment.UpdateAsync(assessment);
                    await UpdateAssignmentQuestions(id, selectedQuestionIds);
                    await _unitOfWork.Save();
                await PopulateEditViewBags(assessment);

                return RedirectToAction(nameof(Details), new { id });
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating assessment with ID {id}");
                await PopulateEditViewBags(assessment);
                return View(assessment);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await SetViewDataCounts();
                var assessment = await _unitOfWork.Assessment.GetByIdAsync(id);
                if (assessment == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                // Delete related assessment questions first
                var assessmentQuestions = await _unitOfWork.AssignmentQuestion.GetAllAsync(aq => aq.AssignmentID == id);
                foreach (var aq in assessmentQuestions)
                {
                     _unitOfWork.AssignmentQuestion.DeleteAsync(aq);
                }

                 _unitOfWork.Assessment.DeleteAsync(assessment);
                await _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting assessment with ID {id}");
                return RedirectToAction(nameof(Index));
            }
        }

        #region Helper Methods

        private async Task PopulateCreateViewBags()
        {
            var questions = await _unitOfWork.questions.GetAllAsync();
            ViewBag.QuestionsList = questions;
            ViewBag.Questions = questions.Select(q => new SelectListItem
            {
                Value = q.QuestionID.ToString(),
                Text = q.QuestionText
            }).ToList();

            var courses = await _unitOfWork.Course.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "ID", "Title");

            var lessons = await _unitOfWork.Lesson.GetAllAsync();
            ViewBag.Lessons = new SelectList(lessons, "ID", "Title");
        }

        private async Task PopulateEditViewBags(Assignment assessment)
        {
            var questions = await _unitOfWork.questions.GetAllAsync();
            ViewBag.AllQuestions = questions.Select(q => new SelectListItem
            {
                Value = q.QuestionID.ToString(),
                Text = q.QuestionText,
                Selected = assessment.assignment_Question?.Any(aq => aq.QuestionID == q.QuestionID) ?? false
            }).ToList();

            var courses = await _unitOfWork.Course.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "ID", "Title");

            var lessons = await _unitOfWork.Lesson.GetAllAsync();
            ViewBag.Lessons = new SelectList(lessons, "ID", "Title");
        }

        private async Task UpdateAssignmentQuestions(int assignmentId, List<int> selectedQuestionIds)
        {
            var existingQuestions = await _unitOfWork.AssignmentQuestion.GetAllAsync(
                aq => aq.AssignmentID == assignmentId);

            // Remove unselected questions
            var questionsToRemove = existingQuestions
                .Where(aq => selectedQuestionIds == null || !selectedQuestionIds.Contains(aq.QuestionID))
                .ToList();

            foreach (var question in questionsToRemove)
            {
                _unitOfWork.AssignmentQuestion.DeleteAsync(question);
            }

            // Add new questions
            if (selectedQuestionIds != null)
            {
                var existingQuestionIds = existingQuestions
                    .Select(aq => aq.QuestionID)
                    .ToHashSet();

                var questionsToAdd = selectedQuestionIds
                    .Where(qId => !existingQuestionIds.Contains(qId))
                    .Select(qId => new assignment_question
                    {
                        AssignmentID = assignmentId,
                        QuestionID = qId,
                        mark = 1 // Default mark
                    });

                foreach (var question in questionsToAdd)
                {
                    await _unitOfWork.AssignmentQuestion.AddAsync(question);
                }
            }
        }

        #endregion
    }
}