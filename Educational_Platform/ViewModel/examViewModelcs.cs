using Data_access_layer.model;

namespace Educational_Platform.ViewModel
{
    public class CreateExamViewModel
    {
        public Exam Exam { get; set; }
        public IEnumerable<Questions> AvailableQuestions { get; set; }
        public IEnumerable<Course> AvailableCourses { get; set; }
        public List<int> SelectedQuestionIds { get; set; }
    }

    public class EditExamViewModel
    {
        public Exam Exam { get; set; }
        public IEnumerable<Questions> AvailableQuestions { get; set; }
        public IEnumerable<Course> AvailableCourses { get; set; }
        public List<int> SelectedQuestionIds { get; set; }
    }

    public class ManageQuestionsViewModel
    {
        public Exam Exam { get; set; }
        public IEnumerable<examQuestion> ExamQuestions { get; set; }
    }
}
