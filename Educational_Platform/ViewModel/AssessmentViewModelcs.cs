using Data_access_layer.model;

namespace Educational_Platform.ViewModel
{
    public class AssessmentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? DurationMinutes { get; set; } // Optional duration in minutes
        public string CourseName { get; set; }
        public string LessonName { get; set; }
        public bool IsSubmitted { get; set; }
        public ICollection<assignment_question> Questions { get; set; } = new List<assignment_question>();

        // Helper property for display

    }
}
