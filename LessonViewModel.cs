public class LessonViewModel
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string VideoURL { get; set; }
    public string SupportingFiles { get; set; }
    public string TaskFileName { get; set; }
    public DateTime Create_date { get; set; }

    // Add this property to fix the issue
    public IFormFile videoFile { get; set; }
}
