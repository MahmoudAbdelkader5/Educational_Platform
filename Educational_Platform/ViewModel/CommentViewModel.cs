public class CommentViewModel
{
    public int LessonID { get; set; }
    public int StudentID { get; set; }
    public string Content { get; set; }
    public DateTime CommentDate { get; set; }
    public string? StudentName { get; set; }
    public string? StudentPhoto { get; set; }
}
