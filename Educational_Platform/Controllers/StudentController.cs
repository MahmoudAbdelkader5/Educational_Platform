using AutoMapper;
using Business_logic_layer.interfaces;
using Business_logic_layer.Repository;
using Data_access_layer.model;
using Educational_Platform.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Educational_Platform.Controllers
{
    public class StudentController : Controller
    {
        private readonly IunitofWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public StudentController(IunitofWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: Update Profile
        [HttpGet]
        public async Task<IActionResult> StudentProfile()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Ensure the user is linked to a student profile
            if (user.StudentId == null)
            {
                return NotFound("Student profile not found.");
            }

            // Retrieve student information
            var student = await _unitOfWork.Student.GetByIdAsync(user.StudentId.Value);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            // Retrieve courses the student is enrolled in
            var studentCourses = await _unitOfWork.student_CourseRepo
                .GetAllAsync(e => e.StudentID == student.ID, includeProperties: "Course");

            // Retrieve exam results for the student
            var examResults = await _unitOfWork.student_Exam
                .GetAllAsync(e => e.StudentID == student.ID, includeProperties: "Exam");
            // Calculate total score across all exams (if needed for display)
            var totalScoreAcrossAllExams = examResults.Sum(e => e.Score);

            // Map exam results to view model
            var examResultViewModels = examResults.Select(e => new ExamResultViewModel
            {
                ExamTitle = e.Exam.Title,
                Score = (int)e.Score, // No need to cast to int unless necessary
                TotalQuestions = e.Exam.ExamQuestions.Count,
                ExamDate = e.ExamDate,
                CorrectAnswers = e.Exam.ExamQuestions.Count(q => q.Question.Answer == e.Score.ToString()), // Assuming Score is the number of correct answers
                Percentage = (double)(e.Score / examResults.Sum(er => er.Score)) * 100, // Assuming Score is out of total questions
            }).ToList();

            // Map data to the StudentProfileViewModel
            var studentProfileViewModel = new StudentProfileViewModel
            {
                Id = student.ID,
                Email = student.Email,
                FirstName = student.Name,
                PhoneNumber = student.PhoneNumber,
                FatherPhone = student.FatherPhone,
                GradeLevel = student.GradeLevel,
                CurrentProfilePicture = student.ProfilePicture,
                EnrolledCourses = studentCourses.Select(e => new CourseViewModel
                {
                    ID = e.Course.ID,
                    Title = e.Course.Title,
                    Description = e.Course.Description,
                    Duration = e.Course.Duration,
                    Price = e.Course.Price,
                    Image = e.Course.Image
                }).ToList(),
                ExamResults = examResultViewModels // Add this property to your StudentProfileViewModel
            };

            return View(studentProfileViewModel);
        }
        // POST: Update Profile
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateStudentProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var student = await _unitOfWork.Student.GetByIdAsync(user.StudentId.Value);
                if (student == null)
                {
                    return NotFound("Student not found.");
                }


                // Handle profile picture - use existing one if no new file was uploaded
                string profilePicturePath = model.CurrentProfilePicture; // This will keep the current picture by default

                if (model.ProfilePictureFile != null && model.ProfilePictureFile.Length > 0)
                {
                    try
                    {
                        // Delete old picture if exists and it's not the default avatar
                        if (!string.IsNullOrEmpty(student.ProfilePicture) &&
                            !student.ProfilePicture.Equals("avatar.png", StringComparison.OrdinalIgnoreCase))
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "files", "studentImage", student.ProfilePicture);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload new picture
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "files", "studentImage");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfilePictureFile.FileName);
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ProfilePictureFile.CopyToAsync(fileStream);
                        }

                        profilePicturePath = uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ProfilePictureFile", "Failed to update profile picture: " + ex.Message);
                        return View(model);
                    }
                }

                // Update student properties
                student.Name = model.Name;
                student.PhoneNumber = model.PhoneNumber;
                student.FatherPhone = model.FatherPhone;
                student.GradeLevel = model.GradeLevel;
                student.ProfilePicture = profilePicturePath; // This will either be the new picture or the current one

                // Update the student in the database
                _unitOfWork.Student.UpdateAsync(student);
                await _unitOfWork.SaveAsync();




                // Rest of your update logic...

                TempData["SuccessMessage"] = "تم تحديث الملف الشخصي بنجاح";
                return RedirectToAction("StudentProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ التغييرات: " + ex.Message);
                return View(model);
            }
        }




        public async Task<IActionResult> AvailableExams(int courseId)
        {
            // التحقق من أن الطالب مسجل في الكورس
            var user = await _userManager.GetUserAsync(User);
            var student = await _unitOfWork.Student.GetByIdAsync(user.StudentId.Value);
            var enrolledCourses = await _unitOfWork.student_CourseRepo.GetAllAsync(
                         filter: sc => sc.StudentID == student.ID && sc.PaymentStatus == "Completed", // تم الدفع
                         includeProperties: "Course"  // تضمين معلومات الكورس
                     );



            var exams = await _unitOfWork.Exam.GetAllAsync(
                             filter: e => e.CourseId == courseId,
                             includeProperties: "Course"
                         );

            //// إنشاء ViewModel يحتوي على الامتحانات وعدد الأسئلة
            //var examViewModels = new List<ExamWithQuestionCountViewModel>();

            //foreach (var exam in exams)
            //{
            //    var questionCount = await _unitOfWork.ExamQuestion.GetCountAsync(eq => eq.ExamID == exam.Id);

            //    examViewModels.Add(new ExamWithQuestionCountViewModel
            //    {
            //        Exam = exam,
            //        QuestionCount = questionCount
            //    });
            //}

            return View(exams);
        }


        // بدء الامتحان
        public async Task<IActionResult> TakeExam(int examId)
        {
            var exam = await _unitOfWork.Exam.GetFirstOrDefaultAsync(
                e => e.Id == examId,
                includeProperties: "ExamQuestions.Question");

            if (exam == null)
            {
                TempData["ErrorMessage"] = "الامتحان غير موجود.";
                return RedirectToAction(nameof(AvailableExams));
            }

            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitExam(int examId, string answersJson)
        {
            var answers = JsonConvert.DeserializeObject<Dictionary<int, string>>(answersJson);

            var user = await _userManager.GetUserAsync(User);
            var student = await _unitOfWork.Student.GetByIdAsync(user.StudentId.Value);
            int studentId = student.ID;
            int totalScore = 0;
            int correctAnswers = 0;

            try
            {
                // Deserialize the JSON to dictionary
                // Process answers and calculate score
                var examQuestions = await _unitOfWork.ExamQuestion.GetAllAsync(eq => eq.ExamID == examId);

                foreach (var answer in answers)
                {
                    var EXquestion = examQuestions.FirstOrDefault(q => q.QuestionID == answer.Key);
                    int qid = EXquestion.QuestionID;
                    var question = await _unitOfWork.questions.GetByIdAsync(qid);
                    if (question != null)
                    {
                        bool isCorrect;
                        string qw = "-1";
                        //= question.Question.Answer == answer.Value;
                        if (question.Answer == "A")
                        {
                            qw = "1";
                        }
                        else if (question.Answer == "B")
                        {
                            qw = "2";
                        }
                        else if (question.Answer == "C")
                        {
                            qw = "3";
                        }
                        else if (question.Answer == "D")
                        {
                            qw = "4";
                        }
                        isCorrect = (qw == answer.Value);



                        if (isCorrect)
                        {
                            totalScore += 1;
                            correctAnswers++;
                        }

                        //string answerText = isCorrect.ToString();
                        //var studentAnswer = new student_answers
                        //{
                        //    examQuestionID = question.ID,
                        //    StudentID = studentId,
                        //    AnswerText = answerText,

                        //};

                        //await _unitOfWork.student_answers.AddAsync(studentAnswer);
                    }
                }

                await _unitOfWork.Save();

                // Save exam result
                var examResult = new Student_Exam
                {
                    ExamID = examId,
                    StudentID = studentId,
                    Score = totalScore,
                    ExamDate = DateTime.Now,
                    //CorrectAnswersCount = correctAnswers,
                    //TotalQuestionsCount = examQuestions.Count
                };

                await _unitOfWork.student_Exam.AddAsync(examResult);
                await _unitOfWork.Save();

                int qn = answers.Count;
                // في الـ Action الأول
                ViewBag.TotalScore = totalScore;
                ViewBag.CorrectAnswers = correctAnswers;
                ViewBag.TotalQuestions = qn;

                //return RedirectToAction("R", new { examId, studentId });
                return View("R");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء حفظ الإجابات، يرجى المحاولة مرة أخرى";
                return RedirectToAction("R", new { examId, studentId });
            }
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