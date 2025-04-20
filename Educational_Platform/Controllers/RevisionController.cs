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

    public class RevisionController : Controller
    {
        private readonly IunitofWork _unitOfWork;
        private readonly IMapper _mapper;

        public RevisionController(IunitofWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> Index(string search)
        {
            IEnumerable<Revision> revisions;

            if (string.IsNullOrEmpty(search))
            {
                revisions = await _unitOfWork.Revision.GetAllAsync();
            }
            else
            {
                revisions = await Task.Run(() => _unitOfWork.Revision.searchCourseBytitle(search).ToList());
            }

            if (revisions == null || !revisions.Any())
            {
                TempData["Message"] = "No revisions found.";
                return View(new List<Revision>());
            }

            return View(revisions);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                ViewBag.Courses = await _unitOfWork.Course.GetAllAsync();

                var revision = await _unitOfWork.Revision.GetByIdAsync(id);
                
                if (revision == null)
                {
                    TempData["ErrorMessage"] = "Revision not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(revision);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving revision details.";
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
        public async Task<IActionResult> Create(RevisionModelView revisionViewModel)
        {
            try
            {
                var revision = _mapper.Map<Revision>(revisionViewModel);

                // Handle file uploads
                if (revisionViewModel.File != null)
                {
                    revision.Files = Helper.Helper.uploadfile(revisionViewModel.File, "file");
                }

                if (revisionViewModel.VideoFile != null)
                {
                    revision.Video = Helper.Helper.uploadfile(revisionViewModel.VideoFile, "video");
                }

                await _unitOfWork.Revision.AddAsync(revision);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = $"Revision '{revision.Title}' created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while creating the revision.";
                await PopulateCoursesViewBag();
                return View(revisionViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var revision = await _unitOfWork.Revision.GetByIdAsync(id);

                if (revision == null)
                {
                    TempData["ErrorMessage"] = "Revision not found.";
                    return RedirectToAction(nameof(Index));
                }

                await PopulateCoursesViewBag();
                var revisionViewModel = _mapper.Map<RevisionModelView>(revision);
                return View(revisionViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the edit form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RevisionModelView revisionViewModel)
        {
            if (id != revisionViewModel.ID)
            {
                TempData["ErrorMessage"] = "Revision ID mismatch.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var revision = _mapper.Map<Revision>(revisionViewModel);

                // Handle file uploads if new files are provided
                if (revisionViewModel.File != null)
                {
                    // Delete old file if exists
                    if (!string.IsNullOrEmpty(revision.Files))
                    {
                        Helper.Helper.deletefile(revision.Files, "file");
                    }
                    revision.Files = Helper.Helper.uploadfile(revisionViewModel.File, "file");
                }

                if (revisionViewModel.VideoFile != null)
                {
                    // Delete old video if exists
                    if (!string.IsNullOrEmpty(revision.Video))
                    {
                        Helper.Helper.deletefile(revision.Video, "video");
                    }
                    revision.Video = Helper.Helper.uploadfile(revisionViewModel.VideoFile, "video");
                }

                 _unitOfWork.Revision.UpdateAsync(revision);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = $"Revision '{revision.Title}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the revision.";
                await PopulateCoursesViewBag();
                return View(revisionViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var revision = await _unitOfWork.Revision.GetByIdAsync(id);

                if (revision == null)
                {
                    TempData["ErrorMessage"] = "Revision not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(revision);
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
                var revision = await _unitOfWork.Revision.GetByIdAsync(id);

                if (revision == null)
                {
                    TempData["ErrorMessage"] = "Revision not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Delete associated files if they exist
                if (!string.IsNullOrEmpty(revision.Files))
                {
                    Helper.Helper.deletefile(revision.Files, "file");
                }
                if (!string.IsNullOrEmpty(revision.Video))
                {
                    Helper.Helper.deletefile(revision.Video, "video");
                }

                 _unitOfWork.Revision.DeleteAsync(revision);
                 _unitOfWork.Save();

                TempData["SuccessMessage"] = $"Revision '{revision.Title}' deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the revision.";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task PopulateCoursesViewBag()
        {
            ViewBag.Courses = await _unitOfWork.Course.GetAllAsync();
        }
    }
}