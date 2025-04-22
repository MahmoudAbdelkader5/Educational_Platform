using AutoMapper;
using Business_logic_layer.interfaces;
using Business_logic_layer.Repository;
using Data_access_layer.model;
using Educational_Platform.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Educational_Platform.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class LessonController : Controller
    {
        private readonly IunitofWork _unitOfWork;
        private readonly IMapper _mapper;

        public LessonController(IunitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                IEnumerable<Lesson> lessons;



                lessons = await _unitOfWork.Lesson
                        .GetAllAsync(includeProperties: "Course");

                ViewData["LessonCount"] = await _unitOfWork.Lesson.GetCountAsync();
               
                return View(lessons);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving lessons.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var lesson = await _unitOfWork.Lesson.GetByIdAsync(id);
                ViewBag.Courses = await _unitOfWork.Course.GetAllAsync();

                if (lesson == null)
                {
                    TempData["ErrorMessage"] = "Lesson not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Map the Lesson entity to LessonViewModel
                var lessonViewModel = _mapper.Map<LessonViewModel>(lesson);

                return View(lessonViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving lesson details.";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                await PopulateCoursesViewBag();
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the create form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonViewModel lessonVm)
        {
            try
            {
                // Validate CourseID
                if (lessonVm.CourseID <= 0)
                {
                    TempData["ErrorMessage"] = "Course selection is required.";
                    await PopulateCoursesViewBag();
                    return View(lessonVm);
                }

                // Manual validation example
                if (string.IsNullOrWhiteSpace(lessonVm.Title))
                {
                    TempData["ErrorMessage"] = "Title is required.";
                    await PopulateCoursesViewBag();
                    return View(lessonVm);
                }

                var lesson = _mapper.Map<Lesson>(lessonVm);

                // Handle file uploads
                if (lessonVm.Files != null)
                {
                    lesson.SupportingFiles = Helper.Helper.uploadfile(lessonVm.Files, "file");
                }
                if (lessonVm.TaskFile != null)
                {
                    lesson.TaskFileName = Helper.Helper.uploadfile(lessonVm.TaskFile, "file");
                }
                if (lessonVm.videoFile != null)
                {
                    lesson.VideoURL = Helper.Helper.uploadfile(lessonVm.videoFile, "video");
                }

                await _unitOfWork.Lesson.AddAsync(lesson);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = $"Lesson '{lesson.Title}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the lesson.";
                await PopulateCoursesViewBag();
                return View(lessonVm);
            }
        }

        

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var lesson = await _unitOfWork.Lesson.GetByIdAsync(id);

                if (lesson == null)
                {
                    TempData["ErrorMessage"] = "Lesson not found.";
                    return RedirectToAction(nameof(Index));
                }

                await PopulateCoursesViewBag();
                var lessonViewModel = _mapper.Map<LessonViewModel>(lesson);
                return View(lessonViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LessonViewModel lessonViewModel)
        {
            if (id != lessonViewModel.ID)
            {
                TempData["ErrorMessage"] = "Lesson ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // Validate CourseID
                if (lessonViewModel.CourseID <= 0)
                {
                    TempData["ErrorMessage"] = "Course selection is required.";
                    await PopulateCoursesViewBag();
                    return View(lessonViewModel);
                }

                // Manual validation example
                if (string.IsNullOrWhiteSpace(lessonViewModel.Title))
                {
                    TempData["ErrorMessage"] = "Title is required.";
                    await PopulateCoursesViewBag();
                    return View(lessonViewModel);
                }

                var lesson = await _unitOfWork.Lesson.GetByIdAsync(id);
                if (lesson == null)
                {
                    TempData["ErrorMessage"] = "Lesson not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Map updated properties from ViewModel to the Lesson entity
                _mapper.Map(lessonViewModel, lesson);

                // Handle file uploads if new files are provided
                if (lessonViewModel.Files != null)
                {
                    // Delete old file if exists
                    if (!string.IsNullOrEmpty(lesson.SupportingFiles))
                    {
                        Helper.Helper.deletefile(lesson.SupportingFiles, "file");
                    }
                    lesson.SupportingFiles = Helper.Helper.uploadfile(lessonViewModel.Files, "file");
                }

                if (lessonViewModel.videoFile != null)
                {
                    // Delete old video if exists
                    if (!string.IsNullOrEmpty(lesson.VideoURL))
                    {
                        Helper.Helper.deletefile(lesson.VideoURL, "video");
                    }
                    lesson.VideoURL = Helper.Helper.uploadfile(lessonViewModel.videoFile, "video");
                }

                if (lessonViewModel.TaskFile != null)
                {
                    // Delete old task file if exists
                    if (!string.IsNullOrEmpty(lesson.TaskFileName))
                    {
                        Helper.Helper.deletefile(lesson.TaskFileName, "file");
                    }
                    lesson.TaskFileName = Helper.Helper.uploadfile(lessonViewModel.TaskFile, "file");
                }

                _unitOfWork.Lesson.UpdateAsync(lesson);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = $"Lesson '{lesson.Title}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the lesson.";
                await PopulateCoursesViewBag();
                return View(lessonViewModel);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var lesson = await _unitOfWork.Lesson.GetByIdAsync(id);

                if (lesson == null)
                {
                    TempData["ErrorMessage"] = "Lesson not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(lesson);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the delete confirmation.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var lesson = await _unitOfWork.Lesson.GetByIdAsync(id);

                if (lesson == null)
                {
                    TempData["ErrorMessage"] = "Lesson not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Delete associated files if they exist
                if (!string.IsNullOrEmpty(lesson.SupportingFiles))
                {
                    Helper.Helper.deletefile(lesson.SupportingFiles, "file");
                }
                if (!string.IsNullOrEmpty(lesson.VideoURL))
                {
                    Helper.Helper.deletefile(lesson.VideoURL, "video");
                }

                _unitOfWork.Lesson.DeleteAsync(lesson);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = $"Lesson '{lesson.Title}' deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the lesson.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task PopulateCoursesViewBag()
        {
            ViewBag.Courses = await _unitOfWork.Course.GetAllAsync();
        }
    }
}