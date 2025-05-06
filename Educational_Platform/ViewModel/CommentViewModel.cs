public class CommentViewModel
{
    public int ? id { get; set; }

    public int LessonID { get; set; }
    public string  ? lessonName { get; set; }
    public int ? StudentID { get; set; }
    public string Content { get; set; }
    public DateTime CommentDate { get; set; }
    public string? StudentName { get; set; }
    public string? StudentPhoto { get; set; }
}
