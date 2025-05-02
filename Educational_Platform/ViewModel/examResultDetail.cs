using Data_access_layer.model;

namespace Educational_Platform.ViewModel
{
    public class ExamResultViewModel
    {
        public string ExamTitle { get; set; }
        public int Score { get; set; }               // Student's obtained score
        public int TotalScore { get; set; }          // Total possible score
        public int CorrectAnswers { get; set; }      // Number of correct answers
        public int TotalQuestions { get; set; }      // Total number of questions
        public DateTime ExamDate { get; set; }
        public double Percentage { get; set; } //
        public ICollection<Student_Exam> studentExam { get; set; } // List of questions in the exam
    }
}
