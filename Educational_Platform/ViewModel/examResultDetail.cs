namespace Educational_Platform.ViewModel
{
    public class ExamResultViewModel
    {
        public int ExamId { get; set; }  // Added to use for details link
        public string ExamTitle { get; set; }
        public int CorrectAnswers { get; set; }  // Number of correct answers (same as Score if 1 point per question)
        public int TotalQuestions { get; set; }  // Total number of questions
        public DateTime ExamDate { get; set; }
        public double Percentage => TotalQuestions > 0 ?
            (CorrectAnswers * 100.0 / TotalQuestions) : 0;

        // Removed redundant properties and unnecessary collection
    }
}