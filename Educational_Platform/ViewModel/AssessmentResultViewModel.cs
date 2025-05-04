namespace Educational_Platform.ViewModel
{
    public class AssessmentResultViewModel
    {
        public int AssessmentId { get; set; }
        public string AssessmentTitle { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime AssessmentDate { get; set; }
        public string Result => Percentage >= 70 ? "Pass" : "Fail";
        public double Percentage => TotalQuestions > 0 ?
            (CorrectAnswers * 100.0 / TotalQuestions) : 0;

        // New properties for course and lesson
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int? LessonId { get; set; }
        public string LessonTitle { get; set; }
    }
}
