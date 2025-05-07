using Business_logic_layer.interfaces;
using Business_logic_layer.Repository;
using Data_access_layer.model;
using Educational_Platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;


public class CommentsController : Controller
{

    public IunitofWork IunitofWork { get; }

    public CommentsController( IunitofWork iunitofWork)
    {
        IunitofWork = iunitofWork;
    }
    public async Task<IActionResult> Index()
    {
        try
        {
            await SetViewDataCounts(); // Assuming this sets some dashboard counts

            // Retrieve comments with related data in a single query
            var comments = await IunitofWork.Comment.GetAllAsync(
    filter: c => c.StudentID != null,
    includeProperties: "Student,Lesson"
           );

            // Map to ViewModel (null checks added)
            var viewModelList = comments.Select(c => new CommentViewModel
            {
                id = c.ID,
                lessonName = c.Lesson?.Title ?? "No Lesson", // Handle null Lesson
                LessonID = c.LessonID,
                StudentID = c.StudentID,
                Content = c.Content,
                CommentDate = c.CommentDate,
                StudentName = c.Student?.Name ?? "Anonymous", // Default if null
                StudentPhoto = c.Student?.ProfilePicture ?? "/images/default-avatar.jpg" // Default image
            }).ToList();

            return View(viewModelList);
        }
        catch (Exception ex)
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    private async Task SetViewDataCounts()
    {
        ViewData["messagesCount"] = await IunitofWork.Message.GetCountAsync();
        ViewData["StudentCount"] = await IunitofWork.student_CourseRepo.GetCountAsync();
        ViewData["LessonCount"] = await IunitofWork.Lesson.GetCountAsync();
        ViewData["RevisionCount"] = await IunitofWork.Revision.GetCountAsync();
        ViewData["questionCount"] = await IunitofWork.questions.GetCountAsync();
        ViewData["CourseCount"] = await IunitofWork.Course.GetCountAsync();
        ViewData["AssessmentCount"] = await IunitofWork.Assessment.GetCountAsync();
        ViewData["examCount"] = await IunitofWork.Exam.GetCountAsync();
        ViewData["CommentCount"] = await IunitofWork.Comment.GetCountAsync();

    }
    // Display list of comments by student
    public async Task<IActionResult> ByStudent(int studentId)
    {
        var comments = (await IunitofWork.Comment.GetAllAsync(c => c.StudentID != null))
            .AsQueryable()
            .Include(c => c.Student)
            .ToList();

        return View(comments);
    }

    // Delete a comment
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await IunitofWork.Comment.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        IunitofWork.Comment.DeleteAsync(comment);
        await IunitofWork.Save();

        return RedirectToAction(nameof(Index)); // or redirect to a list page
    }
}