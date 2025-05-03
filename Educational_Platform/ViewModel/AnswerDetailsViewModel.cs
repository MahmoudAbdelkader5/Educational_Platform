namespace Educational_Platform.ViewModel
{
    public class AnswerDetailsViewModel
    {
        public string ExamTitle { get; set; }
        public DateTime ExamDate { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public List<QuestionResultViewModel> Questions { get; set; }
    }
    public class QuestionResultViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string StudentAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
